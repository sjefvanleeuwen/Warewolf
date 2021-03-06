#pragma warning disable
using System;
using System.Collections.Generic;
using Dev2.Common.Interfaces.Infrastructure.Providers.Validation;
using Dev2.Common.Interfaces.Interfaces;
using Dev2.Providers.Validation.Rules;
using Dev2.Util;
using Dev2.Utilities;
using Dev2.Validation;

namespace Dev2.TO
{
    public class SharepointSearchTo : ValidatedObject, IDev2TOFn, IEquatable<SharepointSearchTo>
    {
        int _indexNum;
        string _searchType;
        bool _isSearchCriteriaEnabled;
        bool _isSearchCriteriaFocused;
        bool _isSearchTypeFocused;
        string _valueToMatch;
        string _from;
        string _to;
        bool _isSearchCriteriaVisible;
        bool _isFromFocused;
        bool _isToFocused;
        string _fieldName;
        string _internalName;

        public SharepointSearchTo()
            : this("Field Name", "Equal", "", 0)
        {
        }

        public SharepointSearchTo(string fieldName, string searchType, string valueToMatch, int indexNum)
            : this(fieldName, searchType, valueToMatch, indexNum, false, "", "")
        {
        }

        public SharepointSearchTo(string fieldName, string searchType, string valueToMatch, int indexNum, bool inserted)
            : this(fieldName, searchType, valueToMatch, indexNum, inserted, "", "")
        {
        }

        public SharepointSearchTo(string fieldName,string searchType, string valueToMatch, int indexNum, bool inserted, string from, string to)
        {
            FieldName = fieldName;
            Inserted = inserted;
            ValueToMatch = valueToMatch;
            SearchType = searchType;
            IndexNumber = indexNum;
            IsSearchCriteriaEnabled = false;
            IsSearchCriteriaVisible = true;
            From = @from;
            To = to;
            IsSearchTypeFocused = false;
        }

        [FindMissing]
        public string From
        {
            get
            {
                return _from;
            }
            set
            {
                _from = value;
                OnPropertyChanged();
                RaiseCanAddRemoveChanged();
            }
        }

        public bool IsFromFocused { get => _isFromFocused; set => OnPropertyChanged(ref _isFromFocused, value); }

        [FindMissing]
        public string To
        {
            get
            {
                return _to;
            }
            set
            {
                _to = value;
                OnPropertyChanged();
                RaiseCanAddRemoveChanged();
            }
        }

        public bool IsToFocused { get => _isToFocused; set => OnPropertyChanged(ref _isToFocused, value); }

        [FindMissing]
        public string ValueToMatch
        {
            get
            {
                return _valueToMatch;
            }
            set
            {
                _valueToMatch = value;
                OnPropertyChanged();
                RaiseCanAddRemoveChanged();
            }
        }

        public bool IsSearchCriteriaFocused { get => _isSearchCriteriaFocused; set => OnPropertyChanged(ref _isSearchCriteriaFocused, value); }

        public string SearchType
        {
            get
            {
                return _searchType;
            }
            set
            {
                if (value != null)
                {
                    _searchType = FindRecordsDisplayUtil.ConvertForDisplay(value);
                    OnPropertyChanged();
                    RaiseCanAddRemoveChanged();
                }
            }
        }

        public bool IsSearchTypeFocused { get => _isSearchTypeFocused; set => OnPropertyChanged(ref _isSearchTypeFocused, value); }

        void RaiseCanAddRemoveChanged()
        {            
            OnPropertyChanged("CanRemove");
            OnPropertyChanged("CanAdd");            
        }

        public bool IsSearchCriteriaEnabled
        {
            get
            {
                return _isSearchCriteriaEnabled;
            }
            set
            {
                _isSearchCriteriaEnabled = value;
                OnPropertyChanged();
            }
        }

        public bool IsSearchCriteriaVisible
        {
            get
            {
                return _isSearchCriteriaVisible;
            }
            set
            {
                _isSearchCriteriaVisible = value;
                OnPropertyChanged(ref _isSearchCriteriaVisible, value);
            }
        }

        public int IndexNumber
        {
            get
            {
                return _indexNum;
            }
            set
            {
                _indexNum = value;
                OnPropertyChanged();
            }
        }

        public bool CanRemove() => string.IsNullOrEmpty(FieldName);

        public bool CanAdd() => !string.IsNullOrEmpty(SearchType) && !string.IsNullOrEmpty(FieldName);

        public void ClearRow()
        {
            ValueToMatch = string.Empty;
            SearchType = "";
        }

        public string FieldName
        {
            get
            {
                return _fieldName;
            }
            set
            {
                if (value == null)
                {
                    return;
                }

                _fieldName = value;
                OnPropertyChanged();
                RaiseCanAddRemoveChanged();
            }
        }

        public string InternalName
        {
            get
            {
                return _internalName;
            }
            set
            {
                if(value==null)
                {
                    return;
                }

                _internalName = value;
                OnPropertyChanged();
                RaiseCanAddRemoveChanged();
            }
        }
        public bool Inserted { get; set; }

        public bool IsEmpty() => string.IsNullOrEmpty(SearchType) && string.IsNullOrEmpty(ValueToMatch);

        public override IRuleSet GetRuleSet(string propertyName, string datalist)
        {
            var ruleSet = new RuleSet();
            if (IsEmpty())
            {
                return ruleSet;
            }
            switch (propertyName)
            {
                case "FieldName":
                    if (FieldName.Length == 0)
                    {
                        ruleSet.Add(new IsStringEmptyRule(() => FieldName));
                    }
                    return ruleSet;
                case "ValueToMatch":
                    if (ValueToMatch.Length == 0)
                    {
                        ruleSet.Add(new IsStringEmptyRule(() => ValueToMatch));
                    }
                    ruleSet.Add(new IsValidExpressionRule(() => ValueToMatch, datalist, "1", new VariableUtils()));
                    return ruleSet;
                default:
                    return ruleSet;
            }
        }

#pragma warning disable S1541 // Methods and properties should not be too complex
        public bool Equals(SharepointSearchTo other)
#pragma warning restore S1541 // Methods and properties should not be too complex
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return IndexNumber == other.IndexNumber
                && string.Equals(SearchType, other.SearchType)
                && IsSearchCriteriaEnabled == other.IsSearchCriteriaEnabled
                && IsSearchCriteriaFocused == other.IsSearchCriteriaFocused
                && IsSearchTypeFocused == other.IsSearchTypeFocused
                && string.Equals(ValueToMatch, other.ValueToMatch)
                && string.Equals(From, other.From)
                && string.Equals(To, other.To)
                && IsSearchCriteriaVisible == other.IsSearchCriteriaVisible
                && IsFromFocused == other.IsFromFocused
                && IsToFocused == other.IsToFocused
                && string.Equals(FieldName, other.FieldName)
                && string.Equals(InternalName, other.InternalName)
                && Inserted == other.Inserted;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((SharepointSearchTo)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = IndexNumber;
                hashCode = (hashCode * 397) ^ (SearchType != null ? SearchType.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ IsSearchCriteriaEnabled.GetHashCode();
                hashCode = (hashCode * 397) ^ IsSearchCriteriaFocused.GetHashCode();
                hashCode = (hashCode * 397) ^ IsSearchTypeFocused.GetHashCode();
                hashCode = (hashCode * 397) ^ (ValueToMatch != null ? ValueToMatch.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (From != null ? From.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (To != null ? To.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ IsSearchCriteriaVisible.GetHashCode();
                hashCode = (hashCode * 397) ^ IsFromFocused.GetHashCode();
                hashCode = (hashCode * 397) ^ IsToFocused.GetHashCode();
                hashCode = (hashCode * 397) ^ (FieldName != null ? FieldName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (InternalName != null ? InternalName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Inserted.GetHashCode();
                return hashCode;
            }
        }
    }

    public static class SharepointSearchOptions
    {
        public static List<string> SearchOptions()
        {
            var searchOptions = new List<string>
            {
                "Begins With",
                "In",
                "Contains",
                "=",
                ">",
                ">=",
                "<",
                "<=",
                "<>"
            };
            return searchOptions;
        }

        public static string GetStartTagForSearchOption(string searchOption)
        {
            switch (searchOption)
            {
                case "Begins With":
                    return "<BeginsWith>";
                case "In":
                    return "<In>";
                case "Contains":
                    return "<Contains>";
                case "=":
                    return "<Eq>";
                case ">":
                    return "<Gt>";
                case ">=":
                    return "<Geq>";
                case "<":
                    return "<Lt>";
                case "<=":
                    return "<Leq>";
                case "<>":
                    return "<Neq>";
                default:
                    return null;
            }
        }

        public static string GetEndTagForSearchOption(string searchOption)
        {
            switch (searchOption)
            {
                case "Begins With":
                    return "</BeginsWith>";
                case "In":
                    return "</In>";
                case "Contains":
                    return "</Contains>";
                case "=":
                    return "</Eq>";
                case ">":
                    return "</Gt>";
                case ">=":
                    return "</Geq>";
                case "<":
                    return "</Lt>";
                case "<=":
                    return "</Leq>";
                case "<>":
                    return "</Neq>";
                default:
                    return null;
            }
        }
    }
}