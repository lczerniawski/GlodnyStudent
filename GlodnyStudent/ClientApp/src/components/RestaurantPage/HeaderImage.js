import React, { Component } from 'react';
import './HeaderImage.css';
import imageRestaurant from'../assets/restaurantImage.jpg';

export default class HeaderImage extends Component {
    render() {

        return (
            <div className="headerImage">
                <div className="mainImage">
                    <img src={imageRestaurant} />
                </div>
            </div>
        )
    }
}
