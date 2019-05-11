import React, { Component } from 'react';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import './Home.css';

export class Home extends Component {
  static displayName = Home.name;

  render () {
    return (
      <div className="Welcome">
          <div className="Info">
              <h1>Podaj nam swój adres</h1>
              <h2>a My wskażemy Ci miejsca warte uwagi.</h2>
          </div>
          <div className="Search">
          
          </div>
      </div>
    );
  }
}
