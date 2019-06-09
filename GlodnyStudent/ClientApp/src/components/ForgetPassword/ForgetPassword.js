import React, { Component } from 'react'

export default class ForgetPassword extends Component {


    constructor(props){
        super(props);
        this.state={
            email:""
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
        fetch(address).then((response) => {
          if (response.ok) {
            return response.json();
          } else {
            throw new Error('Restauration not found');
          }
        })
        .then((result) => {

        })
        .catch((error) => {
          console.log(error);
        });
      }


    render() {
        return (
            <div>
                <h3>Resetuj hasło</h3>
                <p>Na podany adres, zostanie wysłany email z likiem do resetowania hasła.</p>
                <form onSubmit={this.handleSubmit} >
                    <input name="email" type="text" onChange={this.handleInputChange} />
                    <input type="submit"/>
                </form>
            </div>
        )
    }
}
