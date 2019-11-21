/*
   Copyright 2011 - 2019 Adrian Popescu.

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

using System.Globalization;
#if !NET20
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Redmine.Net.Api.Extensions;
using Redmine.Net.Api.Types;

namespace Redmine.Net.Api.JSonConverters
{
    internal class IssueRelationConverter : JavaScriptConverter
    {
        #region Overrides of JavaScriptConverter

        /// <summary>
        ///     When overridden in a derived class, converts the provided dictionary into an object of the specified type.
        /// </summary>
        /// <param name="dictionary">
        ///     An <see cref="System.Collections.Generic.IDictionary{TKey, TValue}" /> instance of property data stored
        ///     as name/value pairs.
        /// </param>
        /// <param name="type">The type of the resulting object.</param>
        /// <param name="serializer">The <see cref="System.Web.Script.Serialization.JavaScriptSerializer" /> instance.</param>
        /// <returns>
        ///     The deserialized object.
        /// </returns>
        public override object Deserialize(IDictionary<string, object> dictionary, Type type,
            JavaScriptSerializer serializer)
        {
            if (dictionary != null)
            {
                var issueRelation = new IssueRelation();

                issueRelation.Id = dictionary.GetValue<int>(RedmineKeys.ID);
                issueRelation.IssueId = dictionary.GetValue<int>(RedmineKeys.ISSUE_ID);
                issueRelation.IssueToId = dictionary.GetValue<int>(RedmineKeys.ISSUE_TO_ID);
                issueRelation.Type = dictionary.GetValue<IssueRelationType>(RedmineKeys.RELATION_TYPE);
                issueRelation.Delay = dictionary.GetValue<int?>(RedmineKeys.DELAY);

                return issueRelation;
            }

            return null;
        }

        /// <summary>
        ///     When overridden in a derived class, builds a dictionary of name/value pairs.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <param name="serializer">The object that is responsible for the serialization.</param>
        /// <returns>
        ///     An object that contains key/value pairs that represent the object�s data.
        /// </returns>
        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            var entity = obj as IssueRelation;

            var result = new Dictionary<string, object>();

            if (entity != null)
            {
                result.Add(RedmineKeys.ISSUE_TO_ID, entity.IssueToId.ToString(CultureInfo.InvariantCulture));
                result.Add(RedmineKeys.RELATION_TYPE, entity.Type.ToString());
                if (entity.Type == IssueRelationType.precedes || entity.Type == IssueRelationType.follows)
                    result.WriteValueOrEmpty(entity.Delay, RedmineKeys.DELAY);

                var root = new Dictionary<string, object>();
                root[RedmineKeys.RELATION] = result;
                return root;
            }

            return result;
        }

        /// <summary>
        ///     When overridden in a derived class, gets a collection of the supported types.
        /// </summary>
        public override IEnumerable<Type> SupportedTypes
        {
            get { return new[] {typeof(IssueRelation)}; }
        }

        #endregion
    }
}
#endif