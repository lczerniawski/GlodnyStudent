import React, { Component } from 'react';
import './Layout.css';
import { NavMenu } from './NavMenu';


export class Layout extends Component {
  static displayName = Layout.name;

 

  render () {
    return (
      <div>
        <NavMenu />
        
        <main>
          {this.props.children}
        </main>
        <footer>
          <p className="wow fadeIn" data-wow-duration="2s">2019 &copy; GłodnyStudent.pl. Wszelkie prawa zastrzeżone.</p>
        </footer>
      </div>
    );
  }
}
