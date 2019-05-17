import React, { Component } from 'react'

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
      <div>
            <p>Zebrane pkt: {this.props.rate}</p>
            <button value="1" name="addToRate" onClick={this.handleRate}>[+]</button> <button name="addToRate" value="-1" onClick={this.handleRate}>[-]</button>
      </div>
    )
  }
}
