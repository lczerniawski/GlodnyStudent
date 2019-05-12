import React, { Component } from 'react'

export default class Sort extends Component {

    constructor(props) {
        super(props);
        this.handleInputChange = this.handleInputChange.bind(this);
    }

    handleInputChange(event) {
        this.SortItems();
        this.props.onSetSort(event);
      }



    SortItems(){
        switch(this.props.sort) {
          case 'priceGrowingly':
            this.props.restaurations.sort((a,b) => (parseFloat(a.highestPrice) < parseFloat(b.highestPrice)) ? 1 : ((parseFloat(b.highestPrice) < parseFloat(a.highestPrice)) ? -1 : 0)); 
            break;
          case 'priceDecreasing':
            this.props.restaurations.sort((a,b) => (parseFloat(a.highestPrice) > parseFloat(b.highestPrice)) ? 1 : ((parseFloat(b.highestPrice) > parseFloat(a.highestPrice)) ? -1 : 0)); 
            break;
          case 'distanceDecreasing':
            this.props.restaurations.sort((a,b) => (parseFloat(a.distance) > parseFloat(b.distance)) ? 1 : ((parseFloat(b.distance) > parseFloat(a.distance)) ? -1 : 0)); 
            break;
          case 'distanceGrowingly':
            this.props.restaurations.sort((a,b) => (parseFloat(a.distance) < parseFloat(b.distance)) ? 1 : ((parseFloat(b.distance) < parseFloat(a.distance)) ? -1 : 0)); 
            break;  
          default:
            
        }
      }

  render() {
    return (
        <label>Sortowanie: 
        <select name="sort" onChange={this.handleInputChange}>
          <option value='priceGrowingly'>Cena rosnąca</option>
          <option value='priceDecreasing'>Cena malejąca</option>
          <option value='distanceDecreasing' >Odległość malejąca</option>
          <option value='distanceGrowingly' >Odległość rosnąca</option>
        </select>
      </label>
    )
  }
}
