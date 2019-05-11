import React, { Component } from 'react';
import { Container } from 'reactstrap';
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
            <p>2019 &copy; GłodnyStudent.pl</p>
        </footer>
      </div>
    );
  }
}
