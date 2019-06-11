import React, { Component } from 'react';
import './Registration.css';
import PropTypes from 'prop-types';

export default class Registration extends Component {

    constructor(props) {
        super(props);
        this.state = {
            name:"",
            email:"",
            password:"",
            repeatedPassword:"",
            nameValidResult:false,
            emailValidResult:false,
            passwordValidResult:false,
            repeatedPasswordValidResult:false,
            nameErrorText:"",
            emailErrorText:"",
            passwordErrorText:"",
            repeatedPasswordErrorText:"",
            disabledRepeatedPassword:true,
            disabledSubmit:true,
            typingTimer:null,
            responseMessage:null
          };
          this.handleInputChange = this.handleInputChange.bind(this);
          this.handleSubmit = this.handleSubmit.bind(this);
          this.startCountdownToValidate =this.startCountdownToValidate.bind(this);
          this.clearTheCountdownToValidate = this.clearTheCountdownToValidate.bind(this);
          this.inputValidate = this.inputValidate.bind(this);
      }


      handleSubmit(e){
        e.preventDefault();
       
         fetch('/api/Auth/register', {
         method: 'POST',
         headers: {
           'Accept': 'application/json',
           'Content-Type': 'application/json',
         },
         body: JSON.stringify({
           username:this.state.name,
           email: this.state.email,
           password: this.state.password
         })
       }).then(res => res.json())
       .then((data) => {
          if(data.status === 200){
            
            sessionStorage.setItem('token',data.token);
            sessionStorage.setItem('username',data.username);
            sessionStorage.setItem('id',data.id);
            sessionStorage.setItem('role',data.role);
            this.context.router.history.push(`/`).reload();  
          }else{
            this.setState({
              responseMessage: data.message
            });
          }

       } )
       .catch((err)=>console.log(err));
      }


      static contextTypes = {
        router: PropTypes.object
      }


      handleInputChange(event) {
        const target = event.target;
        const value = target.value;
        const name = target.name;
    
        this.setState({
          [name]: value
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


      inputValidate(event){
        const {password,nameValidResult,emailValidResult,passwordValidResult,repeatedPasswordValidResult} = this.state;
        const errorMessageTarget= event.target.name + "ErrorText";
        const eventValidResult= event.target.name + "ValidResult";
        let validResult = false;
        let otherField  = false;
        let errorMessage = "";

        switch(event.target.name) {
          case "name":
            validResult = (event.target.value.length >= 2)?true:false;
            errorMessage  = (validResult)?"":"Nazwa musi składać sie conajmniej z dwuch znaków.";
            otherField  = emailValidResult && passwordValidResult && repeatedPasswordValidResult;           
            break;
          case "email":
            let reEmail = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
            validResult = reEmail.test(String(event.target.value).toLowerCase());
            errorMessage  = (validResult )?"":"Podano nieprawidłowy adres email.";
            otherField  = nameValidResult && passwordValidResult && repeatedPasswordValidResult; 
            break;
          case "password":
            let rePass =/(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,}/;
            validResult = rePass.test(String(event.target.value));
            errorMessage  = (validResult )?"":"Hasło musi byc nie ktrótsze niż 8 znaków, oraz zawierać conajmniej jedną: wielką litere, małą literę oraz liczbę.";
            otherField  = nameValidResult && emailValidResult && repeatedPasswordValidResult;
            break;
          case "repeatedPassword":
            validResult = (password === event.target.value)?true:false;
            errorMessage  = (validResult )?"":"Podane hasła nie są identyczne"; 
            otherField  = nameValidResult && emailValidResult && passwordValidResult;       
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


  render() {
    return (
      <div className="registration">
        <div className="titlePage">
            <h2 className="wow fadeInLeft" data-wow-duration="1s">Zarejestruj się</h2>
        </div>
        <div className="backgroundDark">
            <div className="containerLight">
              <p className="text wow fadeIn" data-wow-duration="2s">Uzupełnij poniższy formularz rejestracyjny i dołącz do grona Głodnych Studentów!</p>
              {this.state.responseMessage}
              <form  onSubmit={this.handleSubmit} >
                  <div className="label-form wow fadeIn" data-wow-duration="2s">
                    <label className="filedLabel">Nazwa użytkownika</label>
                    <input className="inputStyle" name="name" type="text" onKeyUp={this.startCountdownToValidate} onKeyDown={this.clearTheCountdownToValidate}
                    onBlur={this.inputValidate} onChange={this.handleInputChange}/> 
                    
                    <p className="error">{this.state.nameErrorText}</p>
                  </div>

                  <div className="label-form wow fadeIn" data-wow-duration="2s">
                    <label className="filedLabel">Adres E-mail</label>
                    <input className="inputStyle" name="email" type="email" onKeyUp={this.startCountdownToValidate} onKeyDown={this.clearTheCountdownToValidate} onBlur={this.inputValidate} onChange={this.handleInputChange} /> 
                    
                    <p className="error">{this.state.emailErrorText}</p>
                  </div>

                  <div className="label-form wow fadeIn" data-wow-duration="2s">
                    <label className="filedLabel">Hasło</label>
                    <input className="inputStyle" name="password" type="password" onKeyUp={this.startCountdownToValidate} onKeyDown={this.clearTheCountdownToValidate} onBlur={this.inputValidate} onChange={this.handleInputChange} /> 
                    
                    <p className="error">{this.state.passwordErrorText}</p>
                  </div>

                  <div className="label-form wow fadeIn" data-wow-duration="2s">
                    <label className="filedLabel"> Powtórz hasło</label>
                    <input className="inputStyle" name="repeatedPassword" disabled={this.state.disabledRepeatedPassword} type="password" onKeyUp={this.startCountdownToValidate} onKeyDown={this.clearTheCountdownToValidate} onBlur={this.inputValidate} onChange={this.handleInputChange} /> 
                  
                    <p className="error">{this.state.repeatedPasswordErrorText}</p>
                  </div>

                  <input className="filedLabel wow fadeIn" data-wow-duration="2s" type="submit" disabled={this.state.disabledSubmit}  value="Zarejestruj się" />
              </form>
            </div>
        </div>
      </div>
    )
  }
}
