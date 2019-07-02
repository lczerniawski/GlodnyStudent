import React, { Component } from 'react'
import GusetMenu from './GusetMenu';
import LogInUserMenu from './LogInUserMenu';

export default class NavListMobile extends Component {

    render() {
        const menu = sessionStorage.getItem("token")?
            <LogInUserMenu handleLogOut={this.props.handleLogOut}  toggleAdminPanel={this.props.toggleAdminPanel} />:
            <GusetMenu email={this.props.email} password={this.props.password} handleInputChange={this.props.handleInputChange} handleLogIn={this.props.handleLogIn} />;
        return (
            <div className="menuMobileContainer" >
                {menu}
            </div>
        )
    }
}
