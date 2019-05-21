import React, { Component } from 'react';
import Search from './Search';
import './Home.css';

export class Home extends Component {
 
  
  render () {
    return (
      <div className="welcome">
        <div className="info wow fadeInLeft" data-wow-duration="2s">
          <h1>Podaj nam swój adres</h1>
          <h2>a my wskażemy Ci miejsca warte uwagi</h2>
        </div>
        <div className="search">
          <Search isMain="true" address= {this.props.address} onAddressInput={this.props.onAddressInput} />
        </div>
      </div>
    );
  }
}
