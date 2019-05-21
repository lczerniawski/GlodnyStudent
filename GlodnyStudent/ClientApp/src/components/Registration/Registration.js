import React, { Component } from 'react';
import './Registration.css';

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
            typingTimer:null
          };
          this.handleInputChange = this.handleInputChange.bind(this);
         // this.handleSubmit = this.handleSubmit.bind(this);
          this.startCountdownToValidate =this.startCountdownToValidate.bind(this);
          this.clearTheCountdownToValidate = this.clearTheCountdownToValidate.bind(this);
          this.inputValidate = this.inputValidate.bind(this);
      }


/*       handleSubmit(e){
        e.preventDefault();
        const adr =`api/Restaurants/${this.props.id}/AddReview`;
       
         fetch(adr, {
         method: 'PUT',
         headers: {
           'Accept': 'application/json',
           'Content-Type': 'application/json',
         },
         body: JSON.stringify({
           name:this.state.name,
           email: this.state.email,
           password: this.state.password
         })
       }).then(res => res.json())
       .then((data) => {
          // ustalic  co bedzie zwracane
         this.state.reviews.push(data);

         this.setState({
           reviews:  this.state.reviews
          });
       
       } )
       .catch((err)=>console.log(err));
      } */



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
            validResult = (event.target.value.length > 0)?true:false;
            errorMessage  = (validResult)?"":"To pole jest wymagane.";
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
      <div>
        <form  /* onSubmit={this.handleSubmit} */  >
            <label className="filedLabel">Nazwa użytkownika<input name="name" type="text" onKeyUp={this.startCountdownToValidate} onKeyDown={this.clearTheCountdownToValidate}
            onBlur={this.inputValidate} onChange={this.handleInputChange}/> {this.state.nameErrorText} </label>
            <label className="filedLabel" >Adres E-mail<input name="email" type="email" onKeyUp={this.startCountdownToValidate} onKeyDown={this.clearTheCountdownToValidate}
            onBlur={this.inputValidate} onChange={this.handleInputChange} /> {this.state.emailErrorText} </label>
            <label className="filedLabel" >Hasło<input name="password" type="password" onKeyUp={this.startCountdownToValidate} onKeyDown={this.clearTheCountdownToValidate} 
            onBlur={this.inputValidate} onChange={this.handleInputChange} /> {this.state.passwordErrorText} </label>
            <label className="filedLabel" > Powtórz hasło<input name="repeatedPassword"  disabled={this.state.disabledRepeatedPassword} type="password" onKeyUp={this.startCountdownToValidate} onKeyDown={this.clearTheCountdownToValidate} 
            onBlur={this.inputValidate} onChange={this.handleInputChange} /> {this.state.repeatedPasswordErrorText} </label>
            <input className="filedLabel"   type="submit"   disabled={this.state.disabledSubmit}  value="Zarejestruj się" />
        </form>
      </div>
    )
  }
}
