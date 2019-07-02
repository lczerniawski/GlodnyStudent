import React, { Component } from 'react';
import logo from'../assets/navbarLogo.png';
import GusetMenu from './GusetMenu';
import LogInUserMenu from './LogInUserMenu';
import PropTypes from 'prop-types';
import './NavMenu.css';
import NavListMobile from './NavListMobile';
import ReactCSSTransitionGroup from 'react-addons-css-transition-group';


export class NavMenu extends Component {

  constructor() {
    super();
    this.state={
      email:"",
      password:"",
      responseMessage:null,
      isDesktop: false,
      showMenu:false
    }
    this.handleLogIn = this.handleLogIn.bind(this);
    this.handleLogOut = this.handleLogOut.bind(this);
    this.handleInputChange = this.handleInputChange.bind(this);
    this.updateViewportSize = this.updateViewportSize.bind(this);
    this.showMenu = this.showMenu.bind(this);
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

      let res=null;
      console.log(data.status);
      switch(data.status){
          case 200:
            sessionStorage.setItem('token',data.token);
            sessionStorage.setItem('username',data.username);
            sessionStorage.setItem('id',data.id);
            sessionStorage.setItem('role',data.role);
            window.location.reload();
          break;
          default:
              res = data.message;
      }
        
      this.setState({
         responseMessage: res,
         email:"",
         password:""
        });

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
    sessionStorage.removeItem('role');
   // window.location.reload();
    this.context.router.history.push(`/`);
  }


  componentDidMount() {
    this.updateViewportSize();
    window.addEventListener("resize", this.updateViewportSize);
  }

  componentWillUnmount() {
    window.removeEventListener("resize", this.updateViewportSize);
  }

  updateViewportSize() {
    this.setState({ isDesktop: window.innerWidth >  1100 });
  }
 
  showMenu(){
    this.setState(state => ({
      showMenu: !state.showMenu
    }));
  }


  render () {
    const menuList = this.state.isDesktop?
    sessionStorage.getItem("token")?
      <LogInUserMenu handleLogOut={this.handleLogOut}  toggleAdminPanel={this.props.toggleAdminPanel} />:
      <GusetMenu email={this.state.email} password={this.state.password} handleInputChange={this.handleInputChange} handleLogIn={this.handleLogIn} />:
    <div id="menuIcon" onClick={(e)=>this.showMenu()} >
      <div className="menuIconBars"></div>
      <div className="menuIconBars"></div>
      <div id="menuIconBarsLast" className="menuIconBars" ></div>
    </div> ; 

    const user = (this.state.isDesktop && sessionStorage.getItem("username"))?<p id="Username" className="navMenuListItem">{sessionStorage.getItem("username")}</p>:null;

    const responseMsg = this.state.responseMessage?<p>{this.state.responseMessage}</p>:null;

    const menuMobile = (this.state.showMenu && !this.state.isDesktop)?
     <NavListMobile handleLogOut={this.handleLogOut}  toggleAdminPanel={this.props.toggleAdminPanel} email={this.state.email} password={this.state.password} handleInputChange={this.handleInputChange} handleLogIn={this.handleLogIn}/>:null;

    return (
     
        <header>
          <nav id="navBar">
            <div id="menuNavLogoAndNameLogin" className="menuNavLogoAndName" >
              <a href="/">
                <img id="logo" src={logo} alt ="Głodny Student Logo"/>
              </a>
              {user}
            </div>        
            {responseMsg}
            {menuList}           
          </nav>
          <ReactCSSTransitionGroup
             transitionName="menu"
             transitionEnterTimeout={1000}
             transitionLeaveTimeout={1000}
           >
            {menuMobile}
          </ReactCSSTransitionGroup>             
        </header>  
         
    );
  }
}
