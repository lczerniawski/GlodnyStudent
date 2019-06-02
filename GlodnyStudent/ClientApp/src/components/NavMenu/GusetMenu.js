import React, { Component } from 'react'
import { Link } from 'react-router-dom';

export default class GusetMenu extends Component {

    render() {
        return (
            <div>
                <ul className="navList"> 
                <li><Link to="/">Zapomnialeś hasła?</Link></li>  
                <li><Link to="/Rejestracja">Zarejestruj się</Link></li>
              </ul>
              <form className="formLogin" onSubmit={(e)=>this.props.handleLogIn(e)}>
                  <input type="text" placeholder="Login" />
                  <input type="password" placeholder="Hasło" /> 
                  <input type="submit" value="Zaloguj się"></input>
              </form>
            </div>
        )
    }
}
