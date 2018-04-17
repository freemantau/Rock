﻿// <copyright>
// Copyright by the Spark Development Network
//
// Licensed under the Rock Community License (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.rockrms.com/license
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

using Rock.Data;
using Rock.Model;

namespace Rock.Cache
{
    /// <summary>
    /// Information about a RestController that is required by the rendering engine.
    /// This information will be cached by the engine
    /// </summary>
    [Serializable]
    [DataContract]
    public class CacheRestController : ModelCache<CacheRestController, RestController>
    {

        #region Properties

        private readonly object _obj = new object();

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [DataMember]
        public string ClassName { get; set; }

        /// <summary>
        /// Gets the defined values.
        /// </summary>
        /// <value>
        /// The defined values.
        /// </value>
        public List<CacheRestAction> RestActions
        {
            get
            {
                var restActions = new List<CacheRestAction>();

                if ( _restActionIds == null )
                {
                    lock ( _obj )
                    {
                        if ( _restActionIds == null )
                        {
                            using ( var rockContext = new RockContext() )
                            {
                                _restActionIds = new RestActionService( rockContext )
                                    .Queryable()
                                    .Where( a => a.ControllerId == Id )
                                    .Select( a => a.Id )
                                    .ToList();
                            }
                        }
                    }
                }

                if ( _restActionIds == null ) return restActions;

                foreach ( var id in _restActionIds )
                {
                    var restAction = CacheRestAction.Get( id );
                    if ( restAction != null )
                    {
                        restActions.Add( restAction );
                    }
                }

                return restActions;
            }
        }
        private List<int> _restActionIds;

        #endregion

        #region Public Methods

        /// <summary>
        /// Copies from model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public override void SetFromEntity( IEntity entity )
        {
            base.SetFromEntity( entity );

            var restController = entity as RestController;
            if ( restController == null ) return;

            Name = restController.Name;
            ClassName = restController.ClassName;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represen ts this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Name;
        }

        #endregion

    }
}