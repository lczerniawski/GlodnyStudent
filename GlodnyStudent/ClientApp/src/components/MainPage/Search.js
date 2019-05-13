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

    handleChange(event) {
        this.setState({street: event.target.value});
      }

    handleSubmit(event) {
        
        
       // console.log(`Ulica:  ${this.state.street}`); // Do spawdzenia czy dziala
       this.props.onAddressInput(this.state.street);
        this.context.router.history.push(`/RestaurantList`);
        event.preventDefault();
      }

      static contextTypes = {
        router: PropTypes.object
      }


  render() {
    return (
        <form id="searchContainer" onSubmit={this.handleSubmit}>
          <input id="searchInput" type="text"  placeholder="Tu wpisz adres" onChange={this.handleChange} />
          <input id="searchBtn" type="submit" value="Szukaj"/>
        </form>
    )
  }


}