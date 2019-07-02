import React, { Component } from 'react';
import Search from './Search';
import './Home.css';

export class Home extends Component {
 
  
  render () {
    return (
      <div id="HomeBackground" >
        <div id="HomePageContent">
          <div id="HomePageText" >
            <h1>Podaj nam swój adres</h1>
            <h2>a my wskażemy Ci miejsca w Warszawie,<br/>warte Twojej uwagi</h2>
          </div>
          <div id="HomePageSearch">
            <Search isMain="true"/>
          </div>
        </div>
      </div>
    );
  }
}
