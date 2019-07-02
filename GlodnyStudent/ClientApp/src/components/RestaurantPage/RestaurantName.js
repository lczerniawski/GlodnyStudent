import React, { Component } from 'react';

export default class RestaurantName extends Component {

    constructor(props){
        super(props);
        this.state={
            name:""
        }
    }

    render() {
        const editInput = this.props.ownerLogIn&&sessionStorage.getItem("id")?
        <div>
            <input type="text" name="name" onChange={(e)=>this.setState({name:e.target.value})} placeholder="Podaj nową nazwę restauracji"/>
            <button onClick={(e)=>this.props.setName(e,this.state.name,`${this.props.restaurantId}/UpdateName`)}><span ></span></button> {/* className="fas fa-check" */}
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
