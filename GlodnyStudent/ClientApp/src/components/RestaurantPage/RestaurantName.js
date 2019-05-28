import React, { Component } from 'react'

export default class RestaurantName extends Component {

    constructor(props){
        super(props);
        this.state={
            name:""
        }
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    handleSubmit(e){    
        this.props.setName("name",this.state.name);
        e.preventDefault();
    }

    render() {
        return (
            <div>
                <h2>{this.props.name}</h2>
                <input type="text" name="name" onChange={(e)=>this.setState({name:e.target.value})}  />
                <button onClick={this.handleSubmit}>Zapisz</button>
            </div>
        )
    }
}
