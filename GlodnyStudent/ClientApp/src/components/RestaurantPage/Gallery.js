import React, { Component } from 'react';
import './Gallery.css';

export default class Gallery extends Component {

    render() {

        const galleryList = (this.props.gallery.length !== 0)? this.props.gallery.map(img=><li key= {img.id} className="wow fadeIn" data-wow-duration="2s">
          <a data-fancybox="gallery" href={img.filePath}>
                <img src={img.filePath} />
          </a>
            {sessionStorage.getItem("token")?<button className="buttonDelete" name="gallery" value={img.id}  onClick={(e)=>this.props.removeImage(e)}><span>Usuń</span></button>:null}
        </li>):<p className="emptyGallery">Brak zdjęć w galerii.</p>;


      const editInput = this.props.ownerLogIn&&sessionStorage.getItem("id")?
      <div className="label-form">
          <label className="buttonAccept" htmlFor="uploadGallery">Wgraj nowe zdjęcie do galerii</label>
          <input type="file" id="uploadGallery"  accept="image/x-png,image/gif,image/jpeg" onChange={this.props.filesOnChange} />
          <button className="sendFile" type="text" onClick={this.props.uploadJustFile}>Wyślij plik</button>
      </div>:"";


        return (
            <div className="gallery">
                  <h3>Galeria</h3>
                  <ul>
                    {galleryList}
                  </ul>
                  {editInput}
                  {this.props.responseMessageUploadImage}
                  {this.props.responseMessageRemoveImage}
              </div>
        )
    }
}
