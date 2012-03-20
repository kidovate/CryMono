﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.CompilerServices;

using System.Collections.ObjectModel;

using System.Linq;

namespace CryEngine
{
    public class View
	{
		#region Externals
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static uint _CreateView();
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static void _RemoveView(uint viewId);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static uint _GetActiveView();
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static void _SetActiveView(uint viewId);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static ViewParams _GetViewParams(uint viewId);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static void _SetViewParams(uint viewId, ViewParams cam);
		#endregion

		#region Statics
		/// <summary>
		/// Creates a new, empty view.
		/// </summary>
		public static View Create()
		{
			Views.Add(new View(_CreateView()));

			return Views.Last();
		}

		public static void Remove(View view)
		{
			Views.Remove(view);

			_RemoveView(view.Id);
		}

		public static View ActiveView
		{
			get
			{
				var viewId = _GetActiveView();

				var view = Views.FirstOrDefault(x => x.Id == viewId);
				if(view != default(View))
					return view;

				return new View(viewId);
			}
			set
			{
				_SetActiveView(value.Id);
			}
		}
		#endregion

		internal View(uint viewId)
        {
            Id = new EntityId(viewId);
        }

        internal View(EntityId viewId)
        {
            Id = viewId;
        }

        public void SetActiveView()
        {
            ActiveView = this;
        }

        public Vec3 Position { get { return ViewParams.Position; } set { var viewParams = ViewParams; viewParams.Position = value; ViewParams = viewParams; } }
        public Quat Rotation { get { return ViewParams.Rotation; } set { var viewParams = ViewParams; viewParams.Rotation = value; ViewParams = viewParams; } }

        public float FieldOfView { get { return ViewParams.FieldOfView; } set { var viewParams = ViewParams; viewParams.FieldOfView = value; ViewParams = viewParams; } }

        internal ViewParams ViewParams { get { return _GetViewParams(Id); } set { _SetViewParams(Id, value); } }
        public EntityId Id;

		static Collection<View> Views = new Collection<View>();
    }
}