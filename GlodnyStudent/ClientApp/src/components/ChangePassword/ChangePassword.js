import React, { Component } from 'react'

export default class ChangePassword extends Component {

    constructor(props){
        super(props);
        this.state={
            password:"",
            repeatedPassword:"",
            currentPassword:"",
            passwordValidResult:false,
            repeatedPasswordValidResult:false,
            nameValidResult:false,
            passwordErrorText:"",
            repeatedPasswordErrorText:"",
            currentPasswordErrorText:"",
            disabledSubmit:true,
            disabledRepeatedPassword:true,
            typingTimer:null,
            responseMessage:null
        }
        this.inputValidate = this.inputValidate.bind(this);
        this.startCountdownToValidate = this.startCountdownToValidate.bind(this);
        this.clearTheCountdownToValidate = this.clearTheCountdownToValidate.bind(this);
        this.handleInputChange = this.handleInputChange.bind(this);
        this.sendPassword = this.sendPassword.bind(this);
    }



    inputValidate(event){
        const {password,passwordValidResult,repeatedPasswordValidResult,currentPasswordValidResult} = this.state;
        const errorMessageTarget= event.target.name + "ErrorText";
        const eventValidResult= event.target.name + "ValidResult";
        let validResult = false;
        let otherField  = false;
        let errorMessage = "";
    
        switch(event.target.name) {
            case "currentPassword":
                validResult = (event.target.value.length > 0)?true:false;
                errorMessage  = (validResult)?"":"To pole jest wymagane.";
                otherField  =  passwordValidResult && repeatedPasswordValidResult;           
                break;
            case "password":
                let rePass =/(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,}/;
                validResult = rePass.test(String(event.target.value));
                errorMessage  = (validResult )?"":"Hasło musi byc nie ktrótsze niż 8 znaków, oraz zawierać conajmniej jedną: wielką litere, małą literę oraz liczbę.";
                otherField  = repeatedPasswordValidResult && currentPasswordValidResult;
                break;
            case "repeatedPassword":
                validResult = (password === event.target.value)?true:false;
                errorMessage  = (validResult )?"":"Podane hasła nie są identyczne"; 
                otherField  =  passwordValidResult && currentPasswordValidResult;       
                break;
            default:
              }
    
            this.setState({
                disabledSubmit: !(validResult && otherField),
                [errorMessageTarget]:errorMessage, 
                [eventValidResult]:validResult ,
                disabledRepeatedPassword:(event.target.name === "password")?!validResult:!passwordValidResult
                });
    
    }
    
    
    startCountdownToValidate(event){
        event.persist()
        clearTimeout(this.state.typingTimer);
        let Timer = setTimeout(this.inputValidate, 1000,event);
        this.setState({
          typingTimer:Timer
        });
      }
    
      clearTheCountdownToValidate(){
        clearTimeout(this.state.typingTimer);
      }
    
    
      handleInputChange(event) {
        const target = event.target;
        const value = target.value;
        const name = target.name;
    
        this.setState({
          [name]: value
        });
      }
    
    
    
      sendPassword(event) {
        event.preventDefault();
        const adr =`/api/User/ChangePassword`;
       
        fetch(adr, {
                method: "PUT",
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json',
                    'Authorization':'Bearer ' + sessionStorage.getItem("token")
                },
                body: JSON.stringify({
                        username: sessionStorage.getItem("username"),
                        newPassword: this.state.password,
                        oldPassword:this.state.currentPassword
                })
            }).then(res => res.json())
            .then((data) => {
                let res=null;
                let show = this.state.showForm;
                switch(data.status){
                    case 200:
                        res ="Hasło zostało pomyślnie zmienione";
                    break;
                    case 409:
                        res ="Stare hasło nieprawidłowe";
                    break;
                    default:
                        res ="Ojoj coś poszło nie tak";
                }
                  
                this.setState({
                   responseMessage: res,
                   currentPassword:"",
                   password:"",
                   repeatedPassword:""
                  });
                 
            } )
            .catch((err)=>console.log(err))
    }


    render() {
        return (
            <div>
                <div>
                    <h2>Zmiana hasła</h2>
                </div>
                <div>
                    <div>
                        <h2>{this.state.responseMessage}</h2>
                        <form onSubmit={this.sendPassword}>
                            <div>
                                <label>Aktualne hasło:
                                    <input id="currentPassword" name="currentPassword" type="password" value={this.state.currentPassword}
                                    onKeyUp={this.startCountdownToValidate} onKeyDown={this.clearTheCountdownToValidate} onBlur={this.inputValidate} onChange={this.handleInputChange}/>
                                </label>
                                <p className="error">{this.state.currentPasswordErrorText}</p>
                            </div>
                            <div>
                                <label>Nowe hasło:
                                    <input id="password" name="password" type="password" value={this.state.password}
                                    onKeyUp={this.startCountdownToValidate} onKeyDown={this.clearTheCountdownToValidate} onBlur={this.inputValidate} onChange={this.handleInputChange}/>
                                </label>
                                <p>{this.state.passwordErrorText}</p>
                            </div>
                            <div>
                                <label>Powtórz hasło:
                                    <input id="repeatedPassword" name="repeatedPassword" type="password"  disabled={this.state.disabledRepeatedPassword} value={this.state.repeatedPassword}
                                    onKeyUp={this.startCountdownToValidate} onKeyDown={this.clearTheCountdownToValidate} onBlur={this.inputValidate} onChange={this.handleInputChange}/>
                                </label>                           
                                <p>{this.state.repeatedPasswordErrorText}</p>
                            </div>
                            <div>
                                <input type="submit" disabled={this.state.disabledSubmit}  value="Zapisz" />
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        )
    }
}
