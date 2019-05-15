import React, { Component } from 'react'
import './Search.css';
import PropTypes from 'prop-types';

export default class Search extends Component {

    constructor(props) {
        super(props);
    
        this.state = {street: ''};
        this.handleChange = this.handleChange.bind(this);
        this.handleSubmitMain = this.handleSubmitMain.bind(this);
        this.handleSubmitRestaurationList= this.handleSubmitRestaurationList.bind(this);
      }

    handleChange(event) {
        this.setState({street: event.target.value});
      }

    handleSubmitMain(event) {
        
        
       // console.log(`Ulica:  ${this.state.street}`); // Do spawdzenia czy dziala
       this.props.onAddressInput(this.state.street);
        this.context.router.history.push(`/RestaurantList`);
        event.preventDefault();
      }

      static contextTypes = {
        router: PropTypes.object
      }


      handleSubmitRestaurationList(event){
        this.props.onAddressUpdate(this.state.street);
        event.preventDefault();
      }

  render() {
    const sub =this.props.isMain ? this.handleSubmitMain : this.handleSubmitRestaurationList;
    return (
        <form className="searchContainer" onSubmit={sub}>
          <input className="searchInput" type="text"  placeholder="Tu wpisz adres" onChange={this.handleChange} required/>
          <input className="searchBtn" type="submit" value="Szukaj"/>
        </form>
    )
  }


}