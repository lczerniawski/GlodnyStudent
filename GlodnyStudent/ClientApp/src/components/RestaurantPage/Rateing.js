import React, { Component } from 'react'
import './Rateing.css';

export default class Rateing extends Component {

    constructor(props){
        super(props);
        this.handleRate = this.handleRate.bind(this);
    }

    handleRate(e){
        this.props.onRate(e);
    }


  render() {
    return (
      <div className="rating">
          <div className="ratingButtons">
              <button className="rateUp" value="Up" name="addToRate" onClick={this.handleRate}><i className="fas fa-plus fa-xs"></i></button>
              <button className="rateDown" name="addToRate" value="Down" onClick={this.handleRate}><i className="fas fa-minus fa-xs"></i></button>
          </div>
          <p>Zebrane pkt: {this.props.rate}</p>
      </div>
    )
  }
}
