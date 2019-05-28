import React, { Component } from 'react';
import './NavMenu.css';
import logo from'./assets/navbarLogo.png';
import { Link } from 'react-router-dom';

export class NavMenu extends Component {

  handleClick = () => {
    var x = document.getElementById("myTopnav");
    var icoMenu = document.getElementById("menuIcon");

    if (x.className === "topnav hidden") {
      x.className = "topnav visible";
      icoMenu.className = "menuIcon";
    } 
    else {
      x.className = "topnav hidden";
      icoMenu.className = "menuIcon hidden";
    }
  }

  render () {
    return (
      <header>
        <nav className="menuBar wow fadeInDown" data-wow-duration="2s">

            <div className="navLogo">
              <a href="/">
                <img src={logo} alt ="Głodny Student Logo"/>
              </a>
            </div>

            <div className="topnav hidden" id="myTopnav">
              <ul className="navList"> 
                <li><Link to="/">Zapomnialeś hasła?</Link></li>  
                <li><Link to="/Rejestracja">Zarejestruj się</Link></li>
              </ul>
              <form className="formLogin">
                  <input type="text" placeholder="Login" />
                  <input type="password" placeholder="Hasło" /> 
                  <input type="submit" value="Zaloguj się"></input>
              </form>
            </div>
          
            <a id="menuIcon" className="menuIcon" onClick={this.handleClick}>
              <i className="fas fa-bars fa-2x"></i>
            </a>
        </nav>
       

      </header>
    );
  }
}
