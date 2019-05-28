import React, { Component } from 'react';
import './RestaurantName.css';

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
                <div className="changeName">
                    <input type="text" className="inputStyle" name="name" onChange={(e)=>this.setState({name:e.target.value})} placeholder="Podaj nową nazwę restauracji"/>
                    <button className="buttonAccept" onClick={this.handleSubmit}><span class="fas fa-check"></span></button>
                </div>
            </div>
        )
    }
}
