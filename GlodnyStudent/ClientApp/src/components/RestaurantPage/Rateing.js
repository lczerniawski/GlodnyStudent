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
              <button className="rateUp buttonAccept" value="Up" name="addToRate" onClick={this.handleRate}><span>+</span></button>
              <button className="rateDown buttonDelete" name="addToRate" value="Down" onClick={this.handleRate}><span>-</span></button>
          </div>
          <p>Zebrane pkt: {this.props.rate}</p>
      </div>
    )
  }
}
