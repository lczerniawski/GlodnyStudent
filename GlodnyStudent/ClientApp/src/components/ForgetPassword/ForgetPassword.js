import React, { Component } from 'react'

export default class ForgetPassword extends Component {


    constructor(props){
        super(props);
        this.state={
            email:"",
            responseMessage:null
        }
        this.handleInputChange = this.handleInputChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    handleInputChange(event) {
        const target = event.target;
        const value = target.value;
        const name = target.name;
    
        this.setState({
          [name]: value
        });

      }

      handleSubmit(e){
        e.preventDefault();
        const address = `api/User/sendresetmail/${this.state.email}`;
        fetch(address)
        .then((data) => {
             
          let res=null;
                console.log(data.status);
                switch(data.status){
                    case 200:
                        res ="Email został popranie wysłany.";
                    break;
                    case 404:
                        res ="Podany email nie jest skojarzony z żadnym kontem.";
                    break;
                    default:
                        res ="Ojoj coś poszło nie tak.";
                }
                  
                this.setState({
                   responseMessage: res,
                   email:""
                  });
        })
        .catch((error) => {
          console.log(error);
        });
      }


    render() {
        return (
            <div>
                <div>
                    <h2>Resetuj hasło</h2>
                </div>
                <div>
                    <div>
                        <h3>Resetuj hasło</h3>
                        <p>Na podany adres, zostanie wysłany email z likiem do resetowania hasła.</p>
                        <h4 id="response">{this.state.responseMessage}</h4>
                        <form onSubmit={this.handleSubmit} >
                            <input name="email" type="email" value={this.state.email} onChange={this.handleInputChange} />
                            <input type="submit" value="Wyślij" />
                        </form>
                    </div>
                </div>
            </div>
        )
    }
}
