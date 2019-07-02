import React, { Component } from 'react';
import { NavMenu } from './NavMenu/NavMenu';
import AdminPanel from './AdminPanel/AdminPanel';
import './Layout.css';

export class Layout extends Component {

  constructor(props){
    super(props);
    this.state={
      adminPanelOpen:false
    }
    this.toggleAdminPanel = this.toggleAdminPanel.bind(this);
  }


  toggleAdminPanel(e){
    this.setState(prevState => ({
      adminPanelOpen: !prevState.adminPanelOpen
    })); 
  }


  render () {
    return (
      <div id="layoutGlobal">
        <NavMenu  toggleAdminPanel={this.toggleAdminPanel} />
        {(this.state.adminPanelOpen&&(sessionStorage.getItem("role") === "Admin"))?<AdminPanel/>:null}      
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
