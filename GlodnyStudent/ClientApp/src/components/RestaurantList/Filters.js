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
        <div className="filters">
            <div className="form-label wow fadeIn" data-wow-duration="2s">
              <div className="title">Odległość:</div>
              <input type="range" value={this.props.distance} name="distance" min="1" max="20" onChange={this.handleInputChange}/><span className="unit">{this.props.distance} km</span>
            </div>
            <div className="form-label wow fadeIn" data-wow-duration="2s">
              <div className="title">Zakres Cen:</div>
              <input type="range" value={this.props.price}  name="price" min="0" max={this.props.highestPrice} onChange={this.handleInputChange}/><span className="unit">{this.props.price} zł</span>
            </div>

            <div className="form-label typeKitchen wow fadeIn" data-wow-duration="2s">
              <div className="title">Rodzaj kuchni:</div>
              <select name="cuisine" onChange={this.handleInputChange}>
              {cuisineOptions}
              </select>
            </div>
      </div>
    )
  }
}
