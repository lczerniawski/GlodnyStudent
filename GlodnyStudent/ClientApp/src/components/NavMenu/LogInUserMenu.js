import React, { Component } from 'react'
import { Link } from 'react-router-dom';


export default class LogInUserMenu extends Component {
    render() {
        return ( 
            <div>
                <p>Tomke</p>         
                <ul className="navList"> 
                    <li><Link to="/">Zgłoś restaurację</Link></li> 
                    <li><Link to="/">Ustawienia konta</Link></li>  
                    <li onClick={(e) =>this.props.handleLogOut(e)} >Wyloguj</li>
                </ul>
            </div>
        )
    }
}
