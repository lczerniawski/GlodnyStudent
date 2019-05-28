import React, { Component } from 'react';
import imageRestaurant from'../assets/restaurantImage.jpg';

export default class HeaderImage extends Component {
    render() {

        const imageHeader = this.props.headerImage ? this.props.headerImage.dataUrl :imageRestaurant;

        return (
            <div>
                <label>Wgraj nowe zdjęcie nagłówkowe: 
                    <input type="file" name="headerImage" onChange={(e)=>this.props.addImage(e)}/>
                    <button  name="headerImage"  onClick={(e)=>this.props.removeImage(e)}>-</button>
                </label>
                <div className="mainImage">
                    <img src={imageHeader} />
                </div>
            </div>
        )
    }
}
