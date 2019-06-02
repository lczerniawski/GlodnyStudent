import React, { Component } from 'react';
import './NavMenu.css';
import logo from'../assets/navbarLogo.png';
import GusetMenu from './GusetMenu';
import LogInUserMenu from './LogInUserMenu';
import PropTypes from 'prop-types';


export class NavMenu extends Component {

  constructor() {
    super();
    this.handleLogIn = this.handleLogIn.bind(this);
    this.handleLogOut = this.handleLogOut.bind(this);
  }
  
  handleLogIn(event) {
    const data = new FormData(event.target);

   /*  fetch('/api/form-submit-url', {
      method: 'POST',
      body: data,
    }); */

    /* fetch('AdresDoKontrolera', {
      method: 'PUT',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
      },
      body: data
    }).then(res => res.json())
    .then((data) => {

      sessionStorage.setItem('token',data);
    
    } )
    .catch((err)=>console.log(err));   */

    const token = "123abc";
    sessionStorage.setItem('token',token);
    window.location.reload();
  }


  static contextTypes = {
    router: PropTypes.object
  }


  handleLogOut(e){
    sessionStorage.removeItem('token');
    window.location.reload();
  }



  render () {
    const menuList = sessionStorage.getItem("token")?<LogInUserMenu handleLogOut={this.handleLogOut}  />:<GusetMenu handleLogIn={this.handleLogIn} />;
    return (
      <header>
        <nav className="menuBar wow fadeInDown" data-wow-duration="2s">

            <div className="navLogo">
              <a href="/">
                <img src={logo} alt ="Głodny Student Logo"/>
              </a>
            </div>

            <div className="topnav" id="myTopnav">
              {menuList}
            </div>
          
            <a id="menuIcon" className="menuIcon">
              <span className="fas fa-bars fa-2x"></span>
            </a>
        </nav>
       

      </header>
    );
  }
}
