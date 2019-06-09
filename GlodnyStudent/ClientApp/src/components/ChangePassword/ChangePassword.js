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
            typingTimer:null
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
                
            } )
            .catch((err)=>console.log(err))
    }


    render() {
        return (
            <form onSubmit={this.sendPassword}>
                <label>Aktualne hasło:<input name="currentPassword" type="password" 
                onKeyUp={this.startCountdownToValidate} onKeyDown={this.clearTheCountdownToValidate} onBlur={this.inputValidate} onChange={this.handleInputChange}/>{this.state.currentPasswordErrorText}</label>
                <label>Nowe hasło:<input name="password" type="password" 
                onKeyUp={this.startCountdownToValidate} onKeyDown={this.clearTheCountdownToValidate} onBlur={this.inputValidate} onChange={this.handleInputChange}/>{this.state.passwordErrorText}</label>
                <label>Powtórz hasło:<input name="repeatedPassword" type="password"  disabled={this.state.disabledRepeatedPassword}
                onKeyUp={this.startCountdownToValidate} onKeyDown={this.clearTheCountdownToValidate} onBlur={this.inputValidate} onChange={this.handleInputChange}/>{this.state.repeatedPasswordErrorText}</label>
                <input className="filedLabel wow fadeIn" data-wow-duration="2s" type="submit" disabled={this.state.disabledSubmit}  value="Zapisz" />
            </form>
        )
    }
}
