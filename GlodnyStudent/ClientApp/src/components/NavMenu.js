import React, { Component } from 'react';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';
import logo from'./assets/navbarLogo.png';

export class NavMenu extends Component {
  /* static displayName = NavMenu.name;
 */
  /* constructor (props) {
    super(props);

    this.toggleNavbar = this.toggleNavbar.bind(this);
    this.state = {
      collapsed: true
    };
  } */

 /*  toggleNavbar () {
    this.setState({
      collapsed: !this.state.collapsed
    });
  } */

  render () {
    return (
      <header>
       {/*   <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" light>
          <Container className="navBarlist">
            <NavbarBrand tag={Link} to="/"><img src={logo} alt ="logo"/></NavbarBrand>
            <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
            <Collapse className="d-sm-inline-flex flex-sm-row-reverse " isOpen={!this.state.collapsed} navbar>
              <ul className="navbar-nav flex-grow ">
                <input type="text" placeholder="Login" />
                <input type="password" placeholder="Hasło" />
                <NavItem>Zaloguj się</NavItem>
                <NavItem>Zarejestruj się</NavItem>
              </ul>
            </Collapse>
          </Container>
        </Navbar>  */}
        <nav class="menuBar">
            <div className="navLogo">
              <a href="/">
                <img src={logo} alt ="logo"/>
              </a>
            </div>
            <ul className="navList"> 
                <li><a href="#">Zapomnialeś hasła?</a></li>  
                <li><a href="#">Zarejestruj się</a></li>
            </ul>
            <form className="formLogin">
                <input type="text" placeholder="Login" />
                <input type="password" placeholder="Hasło" /> 
                <input type="submit" value="Zaloguj się"></input>
            </form>
            <div className="menuIcon">
                <div className="menuIconBar"></div>
                <div className="menuIconBar"></div>
                <div className="menuIconBar"></div>
            </div>
        </nav>
       

      </header>
    );
  }
}
