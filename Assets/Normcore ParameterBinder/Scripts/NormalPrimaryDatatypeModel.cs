using UnityEngine;
using Normal.Realtime.Serialization;

[RealtimeModel]
public partial class NormalPrimaryDatatypeModel
{
  [RealtimeProperty(1, true, true)] public bool _boolProperty;
  [RealtimeProperty(2, false, true)] public float _floatProperty; 
  
}

/* ----- Begin Normal Autogenerated Code ----- */
public partial class NormalPrimaryDatatypeModel : IModel {
    // Properties
    public bool boolProperty {
        get { return _cache.LookForValueInCache(_boolProperty, entry => entry.boolPropertySet, entry => entry.boolProperty); }
        set { if (value == boolProperty) return; _cache.UpdateLocalCache(entry => { entry.boolPropertySet = true; entry.boolProperty = value; return entry; }); FireBoolPropertyDidChange(value); }
    }
    public float floatProperty {
        get { return _floatProperty; }
        set { if (value == _floatProperty) return; _floatPropertyShouldWrite = true; _floatProperty = value; FireFloatPropertyDidChange(value); }
    }
    
    // Events
    public delegate void BoolPropertyDidChange(NormalPrimaryDatatypeModel model, bool value);
    public event         BoolPropertyDidChange boolPropertyDidChange;
    public delegate void FloatPropertyDidChange(NormalPrimaryDatatypeModel model, float value);
    public event         FloatPropertyDidChange floatPropertyDidChange;
    
    // Delta updates
    private struct LocalCacheEntry {
        public bool boolPropertySet;
        public bool boolProperty;
    }
    
    private LocalChangeCache<LocalCacheEntry> _cache;
    
    private bool _floatPropertyShouldWrite;
    
    public NormalPrimaryDatatypeModel() {
        _cache = new LocalChangeCache<LocalCacheEntry>();
    }
    
    // Events
    public void FireBoolPropertyDidChange(bool value) {
        try {
            if (boolPropertyDidChange != null)
                boolPropertyDidChange(this, value);
        } catch (System.Exception exception) {
            Debug.LogException(exception);
        }
    }
    public void FireFloatPropertyDidChange(float value) {
        try {
            if (floatPropertyDidChange != null)
                floatPropertyDidChange(this, value);
        } catch (System.Exception exception) {
            Debug.LogException(exception);
        }
    }
    
    // Serialization
    enum PropertyID {
        BoolProperty = 1,
        FloatProperty = 2,
    }
    
    public int WriteLength(StreamContext context) {
        int length = 0;
        
        if (context.fullModel) {
            // Mark unreliable properties as clean and flatten the in-flight cache.
            // TODO: Move this out of WriteLength() once we have a prepareToWrite method.
            _floatPropertyShouldWrite = false;
            _boolProperty = boolProperty;
            _cache.Clear();
            
            // Write all properties
            length += WriteStream.WriteVarint32Length((uint)PropertyID.BoolProperty, _boolProperty ? 1u : 0u);
            length += WriteStream.WriteFloatLength((uint)PropertyID.FloatProperty);
        } else {
            // Unreliable properties
            if (context.unreliableChannel) {
                if (_floatPropertyShouldWrite) {
                    length += WriteStream.WriteFloatLength((uint)PropertyID.FloatProperty);
                }
            }
            
            // Reliable properties
            if (context.reliableChannel) {
                LocalCacheEntry entry = _cache.localCache;
                if (entry.boolPropertySet)
                    length += WriteStream.WriteVarint32Length((uint)PropertyID.BoolProperty, entry.boolProperty ? 1u : 0u);
            }
        }
        
        return length;
    }
    
    public void Write(WriteStream stream, StreamContext context) {
        if (context.fullModel) {
            // Write all properties
            stream.WriteVarint32((uint)PropertyID.BoolProperty, _boolProperty ? 1u : 0u);
            stream.WriteFloat((uint)PropertyID.FloatProperty, _floatProperty);
            _floatPropertyShouldWrite = false;
        } else {
            // Unreliable properties
            if (context.unreliableChannel) {
                if (_floatPropertyShouldWrite) {
                    stream.WriteFloat((uint)PropertyID.FloatProperty, _floatProperty);
                    _floatPropertyShouldWrite = false;
                }
            }
            
            // Reliable properties
            if (context.reliableChannel) {
                LocalCacheEntry entry = _cache.localCache;
                if (entry.boolPropertySet)
                    _cache.PushLocalCacheToInflight(context.updateID);
                
                if (entry.boolPropertySet)
                    stream.WriteVarint32((uint)PropertyID.BoolProperty, entry.boolProperty ? 1u : 0u);
            }
        }
    }
    
    public void Read(ReadStream stream, StreamContext context) {
        bool boolPropertyExistsInChangeCache = _cache.ValueExistsInCache(entry => entry.boolPropertySet);
        
        // Remove from in-flight
        if (context.deltaUpdatesOnly && context.reliableChannel)
            _cache.RemoveUpdateFromInflight(context.updateID);
        
        // Loop through each property and deserialize
        uint propertyID;
        while (stream.ReadNextPropertyID(out propertyID)) {
            switch (propertyID) {
                case (uint)PropertyID.BoolProperty: {
                    bool previousValue = _boolProperty;
                    
                    _boolProperty = (stream.ReadVarint32() != 0);
                    
                    if (!boolPropertyExistsInChangeCache && _boolProperty != previousValue)
                        FireBoolPropertyDidChange(_boolProperty);
                    break;
                }
                case (uint)PropertyID.FloatProperty: {
                    float previousValue = _floatProperty;
                    
                    _floatProperty = stream.ReadFloat();
                    _floatPropertyShouldWrite = false;
                    
                    if (_floatProperty != previousValue)
                        FireFloatPropertyDidChange(_floatProperty);
                    break;
                }
                default:
                    stream.SkipProperty();
                    break;
            }
        }
    }
}
/* ----- End Normal Autogenerated Code ----- */
