using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Raven.navigation
{
    public class PathEdge
    {
        

  //positions of the source and destination nodes this edge connects
  private Vector2 m_vSource;
  private Vector2 m_vDestination;

  //the behavior associated with traversing this edge
  private int      m_iBehavior;

private   int      m_iDoorID;

public   PathEdge(Vector2 Source,
           Vector2 Destination,
           int      Behavior) : this(Source, Destination, Behavior, 0)
{
}

        public   PathEdge(Vector2 Source,
           Vector2 Destination,
           int      Behavior,
           int      DoorID){
    m_vSource = Source;
    m_vDestination = Destination;
    m_iBehavior = Behavior;
    m_iDoorID = DoorID;
  }

  public Vector2 Destination(){return m_vDestination;}
  public void     SetDestination(Vector2 NewDest){m_vDestination = NewDest;}
  
  public Vector2 Source(){return m_vSource;}
public   void     SetSource(Vector2 NewSource){m_vSource = NewSource;}

  public int      DoorID(){return m_iDoorID;}
  public int      Behavior(){return m_iBehavior;}
    }
}
