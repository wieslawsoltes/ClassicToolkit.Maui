using Microsoft.Maui.Layouts;

namespace ClassicToolkit.Maui;

    /// <summary>
    /// Viewbox is used to scale single child to fit in the available space.
    /// </summary>
    [ContentProperty(nameof(Child))]
    public class Viewbox : Layout, ILayoutManager
    {
        private readonly ViewboxContainer _containerVisual;

        public static readonly BindableProperty ChildProperty =
            BindableProperty.Create(
                nameof(Child),
                typeof(Element),
                typeof(Viewbox),
                defaultValue: null,
                propertyChanged: ChildPropertyChanged);

        public Element Child
        {
            get => (View)GetValue(ChildProperty);
            set => SetValue(ChildProperty, value);
        }
        
        /// <summary>
        /// Bindable property for the Stretch property.
        /// </summary>
        public static readonly BindableProperty StretchProperty = 
            BindableProperty.Create(nameof(Stretch), typeof(Stretch), typeof(Viewbox), 
                Stretch.Uniform, propertyChanged: OnLayoutPropertyChanged);

        /// <summary>
        /// Bindable property for the StretchDirection property.
        /// </summary>
        public static readonly BindableProperty StretchDirectionProperty = 
            BindableProperty.Create(nameof(StretchDirection), typeof(StretchDirection), typeof(Viewbox), 
                StretchDirection.Both, propertyChanged: OnLayoutPropertyChanged);

        /// <summary>
        /// Initializes a new instance of the Viewbox class.
        /// </summary>
        public Viewbox()
        {
            _containerVisual = new ViewboxContainer();
            Children.Add(_containerVisual);
        }

        /// <summary>
        /// Gets or sets the stretch mode, which determines how child fits into the available space.
        /// </summary>
        public Stretch Stretch
        {
            get => (Stretch)GetValue(StretchProperty);
            set => SetValue(StretchProperty, value);
        }

        /// <summary>
        /// Gets or sets a value controlling in what direction contents will be stretched.
        /// </summary>
        public StretchDirection StretchDirection
        {
            get => (StretchDirection)GetValue(StretchDirectionProperty);
            set => SetValue(StretchDirectionProperty, value);
        }

        private static void ChildPropertyChanged(BindableObject bindableObject, object oldValue, object newValue)
        {
            if (bindableObject is Viewbox viewbox)
            {
                if (oldValue is IView oldChild)
                {
                    //viewbox._containerVisual.Children.Remove(oldChild);
                    viewbox._containerVisual.Child = null;
                }

                if (newValue is IView newChild)
                {
                    //viewbox._containerVisual.Children.Add(newChild);
                    viewbox._containerVisual.Child = newChild;
                }
            }
        }
        
        private static void OnLayoutPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((Viewbox)bindable).InvalidateMeasure();
        }
  
        protected override ILayoutManager CreateLayoutManager()
        {
            return this;
        }

        public Size Measure(double widthConstraint, double heightConstraint)
        {
            var child = _containerVisual;

            // Measure child with infinite constraints to get its natural size
            var childSize = child.Measure(double.PositiveInfinity, double.PositiveInfinity);

            // TODO:
            // var childSize = new Size(child.DesiredSize.Width, child.DesiredSize.Height);
            var availableSize = new Size(widthConstraint, heightConstraint);

            // Calculate the size based on stretch settings
            var size = CalculateSize(availableSize, childSize);

            return size;
        }

        public Size ArrangeChildren(Rect bounds)
        {
            var child = _containerVisual;
            // TODO: child.Child ?
            var childSize = new Size(child.DesiredSize.Width, child.DesiredSize.Height);
            var scale = CalculateScaling(bounds.Size, childSize);

            // Apply scale transform
            _containerVisual.Scale = scale.Width;
            _containerVisual.ScaleX = scale.Width;
            _containerVisual.ScaleY = scale.Height;
            
            // Arrange the child
            child.Arrange(new Rect(new Point(0, 0), childSize));

            return bounds.Size;
        }

        private Size CalculateSize(Size availableSize, Size childSize)
        {
            if (childSize.Width == 0 || childSize.Height == 0)
                return new Size(0, 0);

            var scaleX = availableSize.Width / childSize.Width;
            var scaleY = availableSize.Height / childSize.Height;

            switch (Stretch)
            {
                case Stretch.None:
                    return childSize;
                    
                case Stretch.Fill:
                    return availableSize;
                    
                case Stretch.Uniform:
                    var scale = Math.Min(scaleX, scaleY);
                    return AdjustSize(new Size(childSize.Width * scale, childSize.Height * scale));
                    
                case Stretch.UniformToFill:
                    scale = Math.Max(scaleX, scaleY);
                    return AdjustSize(new Size(childSize.Width * scale, childSize.Height * scale));
                    
                default:
                    return childSize;
            }
        }

        private SizeF CalculateScaling(Size availableSize, Size childSize)
        {
            if (childSize.Width == 0 || childSize.Height == 0)
                return new SizeF(1, 1);

            var scaleX = availableSize.Width / childSize.Width;
            var scaleY = availableSize.Height / childSize.Height;

            switch (Stretch)
            {
                case Stretch.None:
                    return new SizeF(1, 1);
                    
                case Stretch.Fill:
                    return AdjustScale(new SizeF((float)scaleX, (float)scaleY));
                    
                case Stretch.Uniform:
                    var scale = Math.Min(scaleX, scaleY);
                    return AdjustScale(new SizeF((float)scale, (float)scale));
                    
                case Stretch.UniformToFill:
                    scale = Math.Max(scaleX, scaleY);
                    return AdjustScale(new SizeF((float)scale, (float)scale));
                    
                default:
                    return new SizeF(1, 1);
            }
        }

        private Size AdjustSize(Size size)
        {
            switch (StretchDirection)
            {
                case StretchDirection.UpOnly:
                    return new Size(
                        Math.Max(1.0, size.Width),
                        Math.Max(1.0, size.Height));
                    
                case StretchDirection.DownOnly:
                    return new Size(
                        Math.Min(1.0, size.Width),
                        Math.Min(1.0, size.Height));
                    
                case StretchDirection.Both:
                    return size;
                    
                default:
                    return size;
            }
        }

        private SizeF AdjustScale(SizeF scale)
        {
            switch (StretchDirection)
            {
                case StretchDirection.UpOnly:
                    return new SizeF(
                        Math.Max(1.0f, scale.Width),
                        Math.Max(1.0f, scale.Height));
                    
                case StretchDirection.DownOnly:
                    return new SizeF(
                        Math.Min(1.0f, scale.Width),
                        Math.Min(1.0f, scale.Height));
                    
                case StretchDirection.Both:
                    return scale;
                    
                default:
                    return scale;
            }
        }
        /// <summary>
        /// Container for the child view that handles the visual scaling
        /// </summary>
        private class ViewboxContainer : Decorator
        {
            public ViewboxContainer()
            {
                AnchorX = 0;
                AnchorY = 0;
            }
        }
        /*
        /// <summary>
        /// Container for the child view that handles the visual scaling
        /// </summary>
        private class ViewboxContainer : Layout, ILayoutManager
        {
            public ViewboxContainer()
            {
                AnchorX = 0;
                AnchorY = 0;
            }
            
              
            protected override ILayoutManager CreateLayoutManager()
            {
                return this;
            }

            public Size Measure(double widthConstraint, double heightConstraint)
            {
                return new Size(widthConstraint, heightConstraint);
            }

            public Size ArrangeChildren(Rect bounds)
            {
                return bounds.Size;
            }
        }
        */
    }

    /// <summary>
    /// Defines how content is scaled to fill its allocated space.
    /// </summary>
    public enum Stretch
    {
        /// <summary>
        /// Content preserves its original size.
        /// </summary>
        None,

        /// <summary>
        /// Content is resized to fill the destination dimensions. The aspect ratio is not preserved.
        /// </summary>
        Fill,

        /// <summary>
        /// Content is resized to fit in the destination dimensions while preserving its native aspect ratio.
        /// </summary>
        Uniform,

        /// <summary>
        /// Content is resized to fill the destination dimensions while preserving its native aspect ratio.
        /// If the aspect ratio of the destination rectangle differs from the source, the source content is
        /// clipped to fit in the destination dimensions.
        /// </summary>
        UniformToFill
    }

    /// <summary>
    /// Defines in what direction content is scaled.
    /// </summary>
    public enum StretchDirection
    {
        /// <summary>
        /// Only scale the content upward when the content is smaller than the available space.
        /// If the content is larger, no scaling downward is done.
        /// </summary>
        UpOnly,

        /// <summary>
        /// Only scale the content downward when the content is larger than the available space.
        /// If the content is smaller, no scaling upward is done.
        /// </summary>
        DownOnly,

        /// <summary>
        /// Scale in both directions when needed.
        /// </summary>
        Both
    }

// current work
/*
/// <summary>
/// Viewbox is used to scale single child to fit in the available space.
/// </summary>
[ContentProperty(nameof(Child))]
public partial class Viewbox : Layout, ILayoutManager
{
    public static readonly BindableProperty ChildProperty =
        BindableProperty.Create(
            nameof(Child),
            typeof(Element),
            typeof(Viewbox),
            defaultValue: null,
            propertyChanged: ChildPropertyChanged);

    public Element Child
    {
        get => (View)GetValue(ChildProperty);
        set => SetValue(ChildProperty, value);
    }

    private static void ChildPropertyChanged(BindableObject bindableObject, object oldValue, object newValue)
    {
        if (bindableObject is Viewbox viewbox)
        {
            if (oldValue is Element oldChild)
            {
                viewbox.RemoveLogicalChild(oldChild);
            }

            if (newValue is Element newChild)
            {
                viewbox.AddLogicalChild(newChild);
            }
        }
    }
  
    protected override ILayoutManager CreateLayoutManager()
    {
        return this;
    }

    public Size Measure(double widthConstraint, double heightConstraint)
    {
        throw new NotImplementedException();
    }

    public Size ArrangeChildren(Rect bounds)
    {
        throw new NotImplementedException();
    }
}
*/

// Initial working version
/*
public class Viewbox : Layout, ILayoutManager
{
    public static readonly BindableProperty StretchProperty =
        BindableProperty.Create(nameof(Stretch), typeof(Stretch), typeof(Viewbox), Microsoft.Maui.Controls.Stretch.Uniform,
            propertyChanged: (bindable, oldValue, newValue) => ((Viewbox)bindable).InvalidateMeasure());

    public Stretch Stretch
    {
        get => (Stretch)GetValue(StretchProperty);
        set => SetValue(StretchProperty, value);
    }

    public Viewbox()
    {
        Handler?.DisconnectHandler();
    }

    protected override ILayoutManager CreateLayoutManager()
    {
        return this;
    }

    Size ILayoutManager.Measure(double widthConstraint, double heightConstraint)
    {
        var content = Children.FirstOrDefault();
        if (content == null)
            return new Size(0, 0);

        // Get the child's desired size
        var contentSize = content.Measure(double.PositiveInfinity, double.PositiveInfinity);

        // Calculate the size based on stretch mode and constraints
        switch (Stretch)
        {
            case Stretch.None:
                return contentSize;

            case Stretch.Fill:
                return new Size(
                    Math.Min(widthConstraint, contentSize.Width),
                    Math.Min(heightConstraint, contentSize.Height));

            case Stretch.Uniform:
            case Stretch.UniformToFill:
                if (contentSize.Width == 0 || contentSize.Height == 0)
                    return new Size(0, 0);

                var scaleX = widthConstraint / contentSize.Width;
                var scaleY = heightConstraint / contentSize.Height;

                var scale = Stretch == Stretch.Uniform
                    ? Math.Min(scaleX, scaleY)
                    : Math.Max(scaleX, scaleY);

                return new Size(
                    Math.Min(contentSize.Width * scale, widthConstraint),
                    Math.Min(contentSize.Height * scale, heightConstraint));

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    Size ILayoutManager.ArrangeChildren(Rect bounds)
    {
        var content = Children.FirstOrDefault();
        if (content == null)
            return bounds.Size;

        var contentDesiredSize = content.Measure(double.PositiveInfinity, double.PositiveInfinity);

        if (contentDesiredSize.Width == 0 || contentDesiredSize.Height == 0)
            return bounds.Size;

        double scaleX = bounds.Width / contentDesiredSize.Width;
        double scaleY = bounds.Height / contentDesiredSize.Height;
        double scale = 1;

        switch (Stretch)
        {
            case Stretch.None:
                scale = 1;
                break;

            case Stretch.Fill:
                // Content stretches to fill the available space
                content.Arrange(bounds);
                return bounds.Size;

            case Stretch.Uniform:
                scale = Math.Min(scaleX, scaleY);
                break;

            case Stretch.UniformToFill:
                scale = Math.Max(scaleX, scaleY);
                break;
        }

        // Calculate centered position
        var scaledWidth = contentDesiredSize.Width * scale;
        var scaledHeight = contentDesiredSize.Height * scale;
        var x = bounds.X + (bounds.Width - scaledWidth) / 2;
        var y = bounds.Y + (bounds.Height - scaledHeight) / 2;

        // Arrange the content
        content.Arrange(new Rect(x, y, scaledWidth, scaledHeight));

        if (content is VisualElement visualElement)
        {
            visualElement.ScaleX = scale;
            visualElement.ScaleY = scale;
        }

        return bounds.Size;
    }
}
*/

/*
   /// <summary>
   /// Viewbox is used to scale single child to fit in the available space.
   /// </summary>
   public class Viewbox : Control
   {
       private readonly ViewboxContainer _containerVisual;

       /// <summary>
       /// Defines the <see cref="Stretch"/> property.
       /// </summary>
       public static readonly StyledProperty<Stretch> StretchProperty =
           AvaloniaProperty.Register<Viewbox, Stretch>(nameof(Stretch), Stretch.Uniform);

       /// <summary>
       /// Defines the <see cref="StretchDirection"/> property.
       /// </summary>
       public static readonly StyledProperty<StretchDirection> StretchDirectionProperty =
           AvaloniaProperty.Register<Viewbox, StretchDirection>(nameof(StretchDirection), StretchDirection.Both);

       /// <summary>
       /// Defines the <see cref="Child"/> property
       /// </summary>
       public static readonly StyledProperty<Control?> ChildProperty =
           Decorator.ChildProperty.AddOwner<Viewbox>();

       static Viewbox()
       {
           ClipToBoundsProperty.OverrideDefaultValue<Viewbox>(true);
           UseLayoutRoundingProperty.OverrideDefaultValue<Viewbox>(true);
           AffectsMeasure<Viewbox>(StretchProperty, StretchDirectionProperty);
       }

       /// <summary>
       /// Initializes a new instance of the <see cref="Viewbox"/> class.
       /// </summary>
       public Viewbox()
       {
           // The Child control is hosted inside a ViewboxContainer control so that the transform
           // can be applied independently of the Viewbox and Child transforms.
           _containerVisual = new ViewboxContainer();
           _containerVisual.RenderTransformOrigin = RelativePoint.TopLeft;
           ((ISetLogicalParent)_containerVisual).SetParent(this);
           VisualChildren.Add(_containerVisual);
       }

       /// <summary>
       /// Gets or sets the stretch mode, 
       /// which determines how child fits into the available space.
       /// </summary>
       public Stretch Stretch
       {
           get => GetValue(StretchProperty);
           set => SetValue(StretchProperty, value);
       }

       /// <summary>
       /// Gets or sets a value controlling in what direction contents will be stretched.
       /// </summary>
       public StretchDirection StretchDirection
       {
           get => GetValue(StretchDirectionProperty);
           set => SetValue(StretchDirectionProperty, value);
       }

       /// <summary>
       /// Gets or sets the child of the Viewbox
       /// </summary>
       [Content]
       public Control? Child
       {
           get => GetValue(ChildProperty);
           set => SetValue(ChildProperty, value);
       }

       /// <summary>
       /// Gets or sets the transform applied to the container visual that
       /// hosts the child of the Viewbox
       /// </summary>
       internal ITransform? InternalTransform
       {
           get => _containerVisual.RenderTransform;
           set => _containerVisual.RenderTransform = value;
       }

       /// <inheritdoc />
       protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
       {
           base.OnPropertyChanged(change);

           if (change.Property == ChildProperty)
           {
               var (oldChild, newChild) = change.GetOldAndNewValue<Control?>();

               if (oldChild is not null)
               {
                   ((ISetLogicalParent)oldChild).SetParent(null);
                   LogicalChildren.Remove(oldChild);
               }

               _containerVisual.Child = newChild;

               if (newChild is not null)
               {
                   ((ISetLogicalParent)newChild).SetParent(this);
                   LogicalChildren.Add(newChild);
               }

               InvalidateMeasure();
           }
       }

       /// <inheritdoc />
       protected override Size MeasureOverride(Size availableSize)
       {
           var child = _containerVisual;

           child.Measure(Size.Infinity);

           var childSize = child.DesiredSize;

           var size = Stretch.CalculateSize(availableSize, childSize, StretchDirection);

           return size;
       }

       /// <inheritdoc />
       protected override Size ArrangeOverride(Size finalSize)
       {
           var child = _containerVisual;

           var childSize = child.DesiredSize;
           var scale = Stretch.CalculateScaling(finalSize, childSize, StretchDirection);

           InternalTransform = new ImmutableTransform(Matrix.CreateScale(scale.X, scale.Y));

           child.Arrange(new Rect(childSize));

           return childSize * scale;
       }

       /// <summary>
       /// A simple container control which hosts its child as a visual but not logical child.
       /// </summary>
       private class ViewboxContainer : Control
       {
           private Control? _child;

           public Control? Child
           {
               get => _child;
               set
               {
                   if (_child != value)
                   {
                       if (_child is not null)
                           VisualChildren.Remove(_child);

                       _child = value;

                       if (_child is not null)
                           VisualChildren.Add(_child);

                       InvalidateMeasure();
                   }
               }
           }
       }
   }
*/
