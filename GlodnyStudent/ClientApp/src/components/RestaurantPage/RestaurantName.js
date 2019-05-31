import React, { Component } from 'react'

export default class RestaurantName extends Component {

    constructor(props){
        super(props);
        this.state={
            name:""
        }
    }

    render() {
        return (
            <div>
                <h2>{this.props.name}</h2>
                <input type="text" name="name" onChange={(e)=>this.setState({name:e.target.value})}  />
                <button onClick={(e)=>this.props.setName(e,"name",this.state.name,`${this.props.restaurantId}/UpdateName`)}>Zapisz</button>
            </div>
        )
    }
}
