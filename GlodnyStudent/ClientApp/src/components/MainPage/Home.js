import React, { Component } from 'react';
import Search from './Search';
import './Home.css';

export class Home extends Component {
/*   static displayName = Home.name; */
 
  
  render () {
    return (
      <div className="welcome">
        <div className="info">
          <h1>Podaj nam swój adres</h1>
          <h2>a my wskażemy Ci miejsca warte uwagi</h2>
        </div>
        <div className="search">
          <Search isMain="true" onAddressInput={this.props.onAddressInput} />
        </div>
      </div>
    );
  }
}
