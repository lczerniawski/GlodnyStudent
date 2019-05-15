import React, { Component } from 'react'
import './Search.css';
import PropTypes from 'prop-types';

export default class Search extends Component {

    constructor(props) {
        super(props);
    
        this.state = {street: ''};
        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
      }


sendData() {
fetch('https://localhost:44331/api/Home/UserData', {
    method: 'POST',
    headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
        },
    body: JSON.stringify({
                street:this.state.street           
            })
        })
    }


    handleChange(event) {
        this.setState({street: event.target.value});
      }

    handleSubmit(event) {
        
        /* this.sendData(); */ // odkomentowac do wysyalania danych i dac awaita zeby czekal na info zanim przejdzie czy cos
        console.log(`Ulica:  ${this.state.street}`); // Do spawdzenia czy dziala
        this.context.router.history.push(`/RestaurantList`);
        event.preventDefault();
      }

      static contextTypes = {
        router: PropTypes.object
      }


  render() {
    return (
        <form onSubmit={this.handleSubmit}>
          <input type="text"  placeholder="Tu wpisz adres" onChange={this.handleChange}/>
          <input type="submit" value="Szukaj"/>
        </form>
    )
  }


}
