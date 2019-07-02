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
        <div>
            <div>
              <label>Odległość:
                <input type="range" value={this.props.distance} name="distance" min="1" max="20" onChange={this.handleInputChange}/>
                <span>{this.props.distance} km</span>
              </label>
            </div>
            <div>
              <label>Zakres Cen:
                <input type="range" value={this.props.price}  name="price" min="0" max={this.props.highestPrice} onChange={this.handleInputChange}/>
                <span>{this.props.price} zł</span>
              </label>
            </div>
            <div>
              <div>Rodzaj kuchni:</div>
              <select name="cuisine" onChange={this.handleInputChange}>
              {cuisineOptions}
              </select>
            </div>
      </div>
    )
  }
}
