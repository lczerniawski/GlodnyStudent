import React, { Component } from 'react';
import './Gallery.css';

export default class Gallery extends Component {

    render() {

        const galleryList = (this.props.gallery.length !== 0)? this.props.gallery.map((img,i)=><li key= {i} className="wow fadeIn" data-wow-duration="2s">
            <a data-fancybox="gallery" href={img.dataUrl}>
                <img src={img.dataUrl} />
            </a>
            <button className="buttonDelete" name="gallery" value={img.id}  onClick={(e)=>this.props.removeImage(e)}><span>Usuń</span></button>
        </li>):<p className="emptyGallery">Brak zdjęć w galerii.</p>;

        return (
            <div className="gallery">
                  <h3>Galeria</h3>
                  <ul>
                    {galleryList}
                  </ul>
                  <div className="label-form">
                    <label className="buttonAccept" for="uploadGallery">Wgraj nowe zdjęcie do galerii</label>
                    <input id="uploadGallery" type="file" name="gallery" onChange={(e)=>this.props.addImage(e)}/>
                  </div>
              </div>
        )
    }
}
