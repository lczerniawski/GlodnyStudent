import React, { Component } from 'react';
import './Layout.css';
import { NavMenu } from './NavMenu/NavMenu';
import AdminPanel from './AdminPanel/AdminPanel'

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
      <div>
        <NavMenu  toggleAdminPanel={this.toggleAdminPanel} />
        {(this.state.adminPanelOpen&&(sessionStorage.getItem("role") === "Admin"))?<AdminPanel/>:null}      
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
