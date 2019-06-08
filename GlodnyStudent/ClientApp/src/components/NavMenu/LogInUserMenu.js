import React, { Component } from 'react'
import { Link } from 'react-router-dom';


export default class LogInUserMenu extends Component {
    render() {
        return ( 
            <div>
                <p>{sessionStorage.getItem("username")}</p>         
                <ul className="navList">
                    <li onClick={(e)=>this.props.toggleAdminPanel(e)} >Lista użytkowników</li> 
                    <li><Link to="/DodajRestauracje" >Zgłoś restaurację</Link></li> 
                    <li><Link to="/" >Zmień hasło</Link></li>  
                    <li onClick={(e) =>this.props.handleLogOut(e)} >Wyloguj</li>
                </ul>
            </div>
        )
    }
}
