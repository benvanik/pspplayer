// *
// * Copyright (C) 2005 Roger Johansson : http://www.puzzleframework.com
// *
// * This library is free software; you can redistribute it and/or modify it
// * under the terms of the GNU Lesser General Public License 2.1 or later, as
// * published by the Free Software Foundation. See the included license.txt
// * or http://www.gnu.org/copyleft/lesser.html for details.
// *
// *

using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Puzzle.Design
{
	public class ComponaCollectionEditor : CollectionEditor
	{
		#region EditorImplementation

		private ComponaCollectionForm Form = null;

		public void AddObject(object o)
		{
			this.Form.AddObject(o);
		}

		public IDesignerHost DesignerHost
		{
			get
			{
				IDesignerHost designer = (IDesignerHost) this.GetService(typeof (IDesignerHost));
				return designer;
			}
		}

		public void RemoveObject(object o)
		{
			this.Form.RemoveObject(o);
		}


		public ComponaCollectionEditor(Type t) : base(t)
		{
		}


		protected virtual CollectionEditorGui CreateGUI()
		{
			return new CollectionEditorGui();
		}

		protected override CollectionForm CreateCollectionForm()
		{
			Form = new ComponaCollectionForm(this);
			Form.StartPosition = FormStartPosition.CenterScreen;
			return Form;
		}

		#endregion

		#region CollectionForm

		protected class ComponaCollectionForm : CollectionForm
		{
			private CollectionEditorGui GUI = null;
			private ComponaCollectionEditor Editor = null;
			private ArrayList CreatedItems = new ArrayList();
			private ArrayList RemovedItems = new ArrayList();
			private bool IsDirty = false;


			public ComponaCollectionForm(CollectionEditor e) : base(e)
			{
				this.Editor = e as ComponaCollectionEditor;
				GUI = this.Editor.CreateGUI();
				GUI.Visible = true;
				GUI.Dock = DockStyle.Fill;
				this.Controls.Add(GUI);
				this.AcceptButton = GUI.btnOK;
				this.CancelButton = GUI.btnCancel;
				this.Size = new Size(630, 470);
				GUI.Editor = e as ComponaCollectionEditor;

				Type[] types = this.NewItemTypes;
				if (types.Length > 1)
				{
					GUI.btnDropdown.Visible = true;
					GUI.btnDropdown.ContextMenu = new ContextMenu();
					for (int i = 0; (i < types.Length); i ++)
					{
						GUI.btnDropdown.ContextMenu.MenuItems.Add(new TypeMenuItem(types[i], new EventHandler(this.btnDropDownMenuItem_Click)));
					}
				}

				GUI.btnRemove.Click += new EventHandler(this.btnRemove_Click);
				GUI.btnAdd.Click += new EventHandler(this.btnAdd_Click);
				GUI.btnCancel.Click += new EventHandler(this.btnCancel_Click);
				GUI.btnOK.Click += new EventHandler(this.btnOK_Click);
				GUI.btnUp.Click += new EventHandler(this.btnUp_Click);
				GUI.btnDown.Click += new EventHandler(this.btnDown_Click);
				GUI.btnDropdown.Click += new EventHandler(this.btnDropDown_Click);
			}


			public void RemoveObject(object o)
			{
				int index = GUI.lstMembers.Items.IndexOf(o);
				RemovedItems.Add(o);
				object i = o;
				Editor.DestroyInstance(i);
				this.CreatedItems.Remove(i);
				GUI.lstMembers.Items.RemoveAt(GUI.lstMembers.SelectedIndex);
				this.IsDirty = true;
				if (index < GUI.lstMembers.Items.Count)
					GUI.lstMembers.SelectedIndex = index;
				else if (GUI.lstMembers.Items.Count > 0)
					GUI.lstMembers.SelectedIndex = GUI.lstMembers.Items.Count - 1;
			}

			public void AddObject(object o)
			{
				IList e = this.GUI.EditValue as IList;
				e.Add(o);
				this.IsDirty = true;
				this.GUI.lstMembers.Items.Add(o);
				this.CreatedItems.Add(o);
				if (o is Component)
				{
					Component cp = o as Component;
					this.Editor.DesignerHost.Container.Add(cp);
				}
				object[] Items = new object[((uint) GUI.lstMembers.Items.Count)];
				for (int i = 0; (i < Items.Length); i++)
				{
					Items[i] = GUI.lstMembers.Items[i];
				}
			}

			protected void btnUp_Click(object o, EventArgs e)
			{
				int i = GUI.lstMembers.SelectedIndex;
				if (i < 1)
				{
					return;
				}

				this.IsDirty = true;
				int j = GUI.lstMembers.TopIndex;
				object item = GUI.lstMembers.Items[i];
				GUI.lstMembers.Items[i] = GUI.lstMembers.Items[(i - 1)];
				GUI.lstMembers.Items[(i - 1)] = item;
				if (j > 0)
				{
					GUI.lstMembers.TopIndex = (j - 1);

				}
				GUI.lstMembers.ClearSelected();
				GUI.lstMembers.SelectedIndex = (i - 1);
			}

			protected void btnDropDown_Click(object o, EventArgs e)
			{
				GUI.btnDropdown.ContextMenu.Show(GUI.btnDropdown, new Point(0, GUI.btnDropdown.Height));
			}

			protected void btnDropDownMenuItem_Click(object o, EventArgs e)
			{
				TypeMenuItem tmi = o as TypeMenuItem;
//				MessageBox.Show (tmi.ToString ());
				this.CreateAndAddInstance(tmi.Type as Type);
			}

			protected void btnDown_Click(object o, EventArgs e)
			{
				int i = GUI.lstMembers.SelectedIndex;
				if (i >= GUI.lstMembers.Items.Count - 1 && i >= 0)
				{
					return;
				}

				this.IsDirty = true;
				int j = GUI.lstMembers.TopIndex;
				object item = GUI.lstMembers.Items[i];

				GUI.lstMembers.Items[i] = GUI.lstMembers.Items[(i + 1)];
				GUI.lstMembers.Items[(i + 1)] = item;


				if (j < GUI.lstMembers.Items.Count - 1)
				{
					GUI.lstMembers.TopIndex = (j + 1);

				}
				GUI.lstMembers.ClearSelected();
				GUI.lstMembers.SelectedIndex = (i + 1);
			}

			protected void btnRemove_Click(object o, EventArgs e)
			{
				int index = GUI.lstMembers.SelectedIndex;
				RemovedItems.Add(GUI.lstMembers.SelectedItem);
				object i = GUI.lstMembers.SelectedItem;
				Editor.DestroyInstance(i);
				this.CreatedItems.Remove(i);
				GUI.lstMembers.Items.RemoveAt(GUI.lstMembers.SelectedIndex);
				this.IsDirty = true;
				if (index < GUI.lstMembers.Items.Count)
					GUI.lstMembers.SelectedIndex = index;
				else if (GUI.lstMembers.Items.Count > 0)
					GUI.lstMembers.SelectedIndex = GUI.lstMembers.Items.Count - 1;

			}

			protected void btnAdd_Click(object o, EventArgs e)
			{
				this.CreateAndAddInstance(base.NewItemTypes[0]);
			}

			protected void btnCancel_Click(object o, EventArgs e)
			{
				if (IsDirty)
				{
					foreach (object i in this.RemovedItems)
					{
						base.DestroyInstance(i);
					}

//					object[] items = new object[((uint) GUI.lstMembers.Items.Count)];
//					for (int i = 0; i < items.Length; i++)
//					{
//						items[i] = GUI.lstMembers.Items[i];
//					}
//					base.Items = items;


				}
				ClearAll();
			}

			protected void btnOK_Click(object o, EventArgs e)
			{
				if (IsDirty)
				{
					foreach (object i in this.RemovedItems)
					{
						base.DestroyInstance(i);
					}

					object[] items = new object[((uint) GUI.lstMembers.Items.Count)];
					for (int i = 0; i < items.Length; i++)
					{
						items[i] = GUI.lstMembers.Items[i];
					}
					base.Items = items;


				}
				ClearAll();
			}

			private void ClearAll()
			{
				this.CreatedItems.Clear();
				this.RemovedItems.Clear();
				this.IsDirty = false;
			}


			protected override void OnEditValueChanged()
			{
			}

			protected void OnComponentChanged(object o, ComponentChangedEventArgs e)
			{
			}

			protected override DialogResult ShowEditorDialog(IWindowsFormsEditorService edSvc)
			{
				IComponentChangeService Service = null;
				;
				DialogResult Result = DialogResult.Cancel;
				GUI.EditorService = edSvc;


				try
				{
					Service = ((IComponentChangeService) this.Editor.Context.GetService(typeof (IComponentChangeService)));
					if (Service != null)
					{
						Service.ComponentChanged += new ComponentChangedEventHandler(this.OnComponentChanged);

					}
					GUI.EditValue = this.EditValue;
					GUI.Bind();
					GUI.ActiveControl = GUI.lstMembers;
					this.ActiveControl = GUI;

					Result = base.ShowEditorDialog(edSvc);

				}
				finally
				{
					if (Service != null)
					{
						Service.ComponentChanged -= new ComponentChangedEventHandler(this.OnComponentChanged);

					}

				}
				return Result;
			}

			private void CreateAndAddInstance(Type type)
			{
				try
				{
					object NewInstance = base.CreateInstance(type);
					if (NewInstance != null)
					{
						this.IsDirty = true;
						this.CreatedItems.Add(NewInstance);


						GUI.lstMembers.Items.Add(NewInstance);
						GUI.lstMembers.Invalidate();
						GUI.lstMembers.ClearSelected();
						GUI.lstMembers.SelectedIndex = (GUI.lstMembers.Items.Count - 1);

						object[] array1 = new object[((uint) GUI.lstMembers.Items.Count)];
						for (int i = 0; (i < array1.Length); i++)
						{
							array1[i] = GUI.lstMembers.Items[i];

						}
						base.Items = array1;
					}
					this.IsDirty = true;

				}
				catch (Exception x)
				{
					base.DisplayError(x);
				}
			}
		}

		#endregion

		public class TypeMenuItem : MenuItem
		{
			#region PUBLIC PROPERTY TYPE

			private object _Type;

			public object Type
			{
				get { return _Type; }
				set { _Type = value; }
			}

			#endregion

			public TypeMenuItem(object o, EventHandler e) : base()
			{
				this.Text = o.ToString();
				this.Type = o;
				this.Click += e;
			}
		}
	}
}