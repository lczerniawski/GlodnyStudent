import React, { Component } from 'react';
import './NavMenu.css';
import logo from'../assets/navbarLogo.png';
import GusetMenu from './GusetMenu';
import LogInUserMenu from './LogInUserMenu';
import PropTypes from 'prop-types';


export class NavMenu extends Component {

  constructor() {
    super();
    this.state={
      email:"",
      password:""
    }
    this.handleLogIn = this.handleLogIn.bind(this);
    this.handleLogOut = this.handleLogOut.bind(this);
    this.handleInputChange = this.handleInputChange.bind(this);
  }
  

  handleInputChange(event) {
    const target = event.target;
    const value = target.value;
    const name = target.name;

    this.setState({
      [name]: value
    });

  }



  handleLogIn(event) {
    event.preventDefault();
    console.log(`Event: ${event}`);

    
   
    fetch('/api/Auth/login', {
      method: 'POST',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
      },
      body:JSON.stringify({
        email:this.state.email,
        password:this.state.password}),
    }).then(res => res.json())
    .then((data) => {

      if(data.id != null){
        sessionStorage.setItem('token',data.token);
        sessionStorage.setItem('username',data.username);
        sessionStorage.setItem('id',data.id);
      }
      
      window.location.reload();
    } )
    .catch((err)=>console.log(err));  

    
  }


  static contextTypes = {
    router: PropTypes.object
  }


  handleLogOut(e){
    sessionStorage.removeItem('id');
    sessionStorage.removeItem('username');
    sessionStorage.removeItem('token');
    window.location.reload();
  }



  render () {
    const menuList = sessionStorage.getItem("token")?<LogInUserMenu  handleLogOut={this.handleLogOut}  />:<GusetMenu handleInputChange={this.handleInputChange} handleLogIn={this.handleLogIn} />;
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
