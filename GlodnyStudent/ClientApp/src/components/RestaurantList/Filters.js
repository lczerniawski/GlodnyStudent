import React, { Component } from 'react'
import './Filters.css'

export default class Filters extends Component {


    constructor(props) {
        super(props);
        this.handleInputChange = this.handleInputChange.bind(this);
    }

    handleInputChange(event) {
        this.props.onSetFilter(event);
      }

  render() {
    const cuisineOptions = this.props.cuisines.map((cusine)=><option key={cusine} value={cusine}>{cusine}</option>);
    return (
        <div id="filters">
            <label>Odległość<input type="range" value={this.props.distance} name="distance" min="0" max="15" onChange={this.handleInputChange}/>{this.props.distance} km</label>
            <label>Zakres Cen<input type="range" value={this.props.price}  name="price" min="0" max={this.props.highestPrice} onChange={this.handleInputChange}/>{this.props.price} zł</label>
            <select name="cuisine" onChange={this.handleInputChange}>
            {cuisineOptions}
            </select>
      </div>
    )
  }
}
