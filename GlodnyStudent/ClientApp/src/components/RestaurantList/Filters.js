import React, { Component } from 'react'

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
        <div className="filtersContainer">
          <h3>Filtry</h3>
            <label htmlFor="rangeFilter">Odległość: {this.props.distance} km</label>
              <input  id="rangeFilter" className="slider" type="range" value={this.props.distance} name="distance" min="1" max="20" onChange={this.handleInputChange}/>
            <label htmlFor="priceFilter">Zakres Cen: {this.props.price} zł</label>
              <input id="priceFilter" className="slider" type="range" value={this.props.price}  name="price" min="0" max={this.props.highestPrice} onChange={this.handleInputChange}/>  
            <label className="filterDropbox" >Rodzaj kuchni:
              <select  name="cuisine" onChange={this.handleInputChange}>
                {cuisineOptions}
              </select>
            </label>
        </div>
    )
  }
}
