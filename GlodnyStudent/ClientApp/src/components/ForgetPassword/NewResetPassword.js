import React, { Component } from 'react'

export default class NewResetPassword extends Component {

    constructor(props){
        super(props);
        this.state={
            password:"",
            repeatedPassword:"",
            passwordValidResult:false,
            repeatedPasswordValidResult:false,
            passwordErrorText:"",
            repeatedPasswordErrorText:"",
            disabledSubmit:true,
            disabledRepeatedPassword:true,
            typingTimer:null
        }
        this.inputValidate = this.inputValidate.bind(this);
        this.startCountdownToValidate = this.startCountdownToValidate.bind(this);
        this.clearTheCountdownToValidate = this.clearTheCountdownToValidate.bind(this);
        this.handleInputChange = this.handleInputChange.bind(this);
        this.sendPassword = this.sendPassword.bind(this);
    }

    inputValidate(event){
        const {password,passwordValidResult,repeatedPasswordValidResult} = this.state;
        const errorMessageTarget= event.target.name + "ErrorText";
        const eventValidResult= event.target.name + "ValidResult";
        let validResult = false;
        let otherField  = false;
        let errorMessage = "";

        switch(event.target.name) {
            case "password":
                let rePass =/(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,}/;
                validResult = rePass.test(String(event.target.value));
                errorMessage  = (validResult )?"":"Hasło musi byc nie ktrótsze niż 8 znaków, oraz zawierać conajmniej jedną: wielką litere, małą literę oraz liczbę.";
                otherField  = repeatedPasswordValidResult;
                break;
            case "repeatedPassword":
                validResult = (password === event.target.value)?true:false;
                errorMessage  = (validResult )?"":"Podane hasła nie są identyczne"; 
                otherField  =  passwordValidResult;       
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
        const path = decodeURIComponent(window.location.pathname);
        const pathArr = path.split("/");
        const username = pathArr[2];
        const token = pathArr[3];

        const adr =`/api/User/reset`;
       
        fetch(adr, {
                method: "PUT",
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json',
                    'Authorization':'Bearer ' + token
                },
                body: JSON.stringify({
                        username: username,
                    newPassword: this.state.password
                })
            }).then(res => res.json())
            .then((data) => {    
                
            } )
            .catch((err)=>console.log(err))
    }




    render() {
        return (
            <div>
                <form onSubmit={this.sendPassword}>
                    <label>Nowe hasło:<input name="password" type="password" 
                    onKeyUp={this.startCountdownToValidate} onKeyDown={this.clearTheCountdownToValidate} onBlur={this.inputValidate} onChange={this.handleInputChange}/>{this.state.passwordErrorText}</label>
                    <label>Powtórz hasło:<input name="repeatedPassword" type="password"  disabled={this.state.disabledRepeatedPassword}
                    onKeyUp={this.startCountdownToValidate} onKeyDown={this.clearTheCountdownToValidate} onBlur={this.inputValidate} onChange={this.handleInputChange}/>{this.state.repeatedPasswordErrorText}</label>
                    <input className="filedLabel wow fadeIn" data-wow-duration="2s" type="submit" disabled={this.state.disabledSubmit}  value="Zapisz" />
                </form>
            </div>
        )
    }
}
