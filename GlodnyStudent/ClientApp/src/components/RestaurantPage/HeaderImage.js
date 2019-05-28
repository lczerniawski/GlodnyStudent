import React, { Component } from 'react';
import './HeaderImage.css';
import imageRestaurant from'../assets/restaurantImage.jpg';

export default class HeaderImage extends Component {
    render() {

        const imageHeader = this.props.headerImage ? this.props.headerImage.dataUrl :imageRestaurant;

        return (
            <div className="headerImage">
                <div className="uploadImage">
                    <div className="form-label">
                        <label className="buttonAccept" for="file">Wgraj nowe zdjęcie nagłówkowe</label>
                        <input id="file" class="inputfile" type="file" name="headerImage" onChange={(e)=>this.props.addImage(e)}/>
                    </div>
                    <button className="buttonDelete" name="headerImage" onClick={(e)=>this.props.removeImage(e)}><span>Cofnij</span></button>
                </div>
                <div className="mainImage">
                    <img src={imageHeader} />
                </div>
            </div>
        )
    }
}
