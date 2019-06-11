import React, { Component } from 'react';
import './RestaurantName.css';

export default class RestaurantName extends Component {

    constructor(props){
        super(props);
        this.state={
            name:""
        }
    }

    render() {
        const editInput = this.props.ownerLogIn&&sessionStorage.getItem("id")?
        <div className="changeName">
                    <input type="text" className="inputStyle" name="name" onChange={(e)=>this.setState({name:e.target.value})} placeholder="Podaj nową nazwę restauracji"/>
                    <button className="buttonAccept" onClick={(e)=>this.props.setName(e,this.state.name,`${this.props.restaurantId}/UpdateName`)}><span className="fas fa-check"></span></button>
        </div>:"";
        return (
            <div>
                {this.props.responseMessageChangeName}
                <h2>{this.props.name}</h2>
                {editInput}
            </div>
        )
    }
}
