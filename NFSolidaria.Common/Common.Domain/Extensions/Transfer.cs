using Common.Domain;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public static class Transfer
{

    public enum BehaviourTransferTo
    {
        AddNewItemsInCollections,
        DeleteExistsItemAndAddNewItems,
        TransferItemPerItem,
        NoTransferIds,
        NoTransferPrimaryKey,
        TransferZeros,
        TransferNulls,
    }
    public static void TransferTo(this object source, object destination, bool recursive)
    {
        TransferTo<DomainBase>(source, destination, recursive, BehaviourTransferTo.DeleteExistsItemAndAddNewItems);
    }
    public static void TransferTo(this object source, object destination)
    {
        TransferTo<DomainBase>(source, destination, true, BehaviourTransferTo.DeleteExistsItemAndAddNewItems);
    }
    public static void TransferTo(this object source, object destination, bool recursive, BehaviourTransferTo Behaviour)
    {
        TransferTo<DomainBase>(source, destination, recursive, Behaviour);
    }
    public static void TransferTo<T>(this object source, object destination, bool recursive) where T : class
    {
        TransferTo<T>(source, destination, recursive, BehaviourTransferTo.DeleteExistsItemAndAddNewItems);
    }
    public static void TransferTo<T>(this object source, object destination, bool recursive, BehaviourTransferTo Behaviour) where T : class
    {

        var propertys = source.GetType().GetProperties();
        foreach (var item in propertys)
        {
            if (recursive)
            {
                if (item.GetCustomAttribute(typeof(NotMappedAttribute)).IsNotNull())
                    continue;


                if ((item.GetValue(source) as T).IsNotNull())
                {
                    var propDestination = destination.GetType().GetProperty(item.Name);
                    if (propDestination.IsNotNull())
                    {
                        var propDestinationValue = propDestination.GetValue(destination);

                        if (propDestinationValue.IsNull())
                        {
                            var newInstance = Activator.CreateInstance(propDestination.PropertyType);
                            propDestination.SetValue(destination, newInstance);
                            propDestinationValue = propDestination.GetValue(destination);
                        }


                        item.GetValue(source).TransferTo<T>(propDestinationValue, recursive, Behaviour);
                    }
                }
            }

            if (item.IsCollection())
            {
                if (recursive)
                {

                    var collectionSourceValue = item.GetValue(source) as IList;
                    var propDestination = destination.GetType().GetProperty(item.Name);
                    if (propDestination.IsNotNull())
                    {
                        var collectionDestinationValue = propDestination.GetValue(destination) as IEnumerable;

                        if (collectionSourceValue.IsNotNull() && collectionSourceValue.Count > 0)
                        {
                            if (Behaviour == BehaviourTransferTo.AddNewItemsInCollections)
                            {
                                collectionSourceValue.AddRange(collectionDestinationValue);
                                propDestination.SetValue(destination, collectionSourceValue);
                            }
                            else if (Behaviour == BehaviourTransferTo.TransferItemPerItem)
                            {
                                TransferToCollectionItemPerItem(collectionSourceValue, collectionDestinationValue);
                            }
                            else
                                TransferToCollectionDeleteExistsItemAndAddNewItems(source, destination, item, propDestination);
                        }
                    }
                }

            }
            else
            {
                if (item.IsNotNullOrDefault(source, Behaviour == BehaviourTransferTo.TransferZeros, Behaviour == BehaviourTransferTo.TransferNulls))
                {

                    if (Behaviour == BehaviourTransferTo.NoTransferIds)
                        if (item.Name.ToLower().Contains("id"))
                            continue;

                    if (Behaviour == BehaviourTransferTo.NoTransferPrimaryKey)
                        if (string.Format("{0}{1}", destination.GetType().Name, "Id").ToLower() == item.Name.ToLower())
                            continue;

                    var propDestination = destination.GetType().GetProperty(item.Name);
                    if (propDestination.IsNotNull())
                    {
                        var propertyValue = item.GetValue(source);

                        if (propDestination.IsNotNull())
                            if (propDestination.SetMethod.IsNotNull())
                                propDestination.SetValue(destination, propertyValue);
                    }
                }
            }


        }

    }

    private static void TransferToCollectionDeleteExistsItemAndAddNewItems(object source, object destination, PropertyInfo item, PropertyInfo propDestination)
    {
        propDestination.SetValue(destination, item.GetValue(source));
    }

    private static void TransferToCollectionItemPerItem(IList collectionSourceValue, IEnumerable collectionDestinationValue)
    {
        var itemSource = collectionSourceValue.GetEnumerator();
        var itemDestination = collectionDestinationValue.GetEnumerator();


        for (int i = 0; i < collectionSourceValue.Count; i++)
        {
            itemSource.MoveNext();
            itemDestination.MoveNext();
            itemSource.Current.TransferTo(itemDestination.Current);
        }
    }

    private static IList AddRange(this IList collectionSourceValue, IEnumerable collectionDestinationValue)
    {
        var itemDestination = collectionDestinationValue.GetEnumerator();

        while (itemDestination.MoveNext())
            collectionSourceValue.Add(itemDestination.Current);

        return collectionSourceValue;
    }

    private static bool IsMoreThanZero(IEnumerable collectionDestinationValue)
    {
        var itemDestination = collectionDestinationValue.GetEnumerator();
        return itemDestination.MoveNext();
    }


}

