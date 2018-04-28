using Polarith.AI.Move;
using UnityEngine;

namespace Polarith.AI.Package
{
    /// <summary>
    /// This class illustrates how to combine Polarith.AI with Unitys state - and root motion mechanism. It acts as an
    /// example and may be altered, improved or maybe used as base for more elaborate implemenations for specific
    /// animation controllers. By combining Polarith AI and root motion mechanis different animation states like idle,
    /// walk and run are easily manageble with just a few lines.
    /// <para/>
    /// This implementation does two things. First, the character is aligned along the direction received by <see
    /// cref="AIMContext.DecidedDirection"/>. Second, a simple float parameter is passed to the <see cref="Animator"/>
    /// to provide a hint how fast the character should move. This way, the different animation states, like idle or
    /// walking are managed by the animation controller like it should be. Further information can be found here,
    /// https://docs.unity3d.com/Manual/AnimationStateMachines.html. The actual movement of the character is done by the
    /// root motion mechanis (https://docs.unity3d.com/Manual/RootMotion.html).
    /// <para/>
    /// A prerequisite is of course a fitting animation controller setup, like the SimpleWalkCycle.controller in the
    /// Polarith AI package.
    /// <para/>
    /// Note, this is just a script for our example scenes and therefore not part of the actual API. We do not guarantee
    /// that this script is working besides our examples.
    /// </summary>

    public class RootMotionController1 : MonoBehaviour
{
        private GameObject triggerPlayer;
        private bool triggering;
        public GameObject player;
        #region Fields =================================================================================================

        /// <summary>
        /// The animation controller that holds and manages the different animation states. This is a mandatory
        /// reference since the decision of Polarith AI ( <see cref="Context"/>) is applied to this animation
        /// controller. If the reference is <c>null</c>, the component searches for an attached Animator OnEnable.
        /// </summary>
        [Tooltip("The animation controller that holds and manages the different animation states. This is a " +
        "mandatory reference since the decision of Polarith AI (see 'Context') is applied to this animation " +
        "controller. If the reference is null, the component searches for an attached Animator OnEnable.")]
    //public Animator Animator;
    // public Rigidbody Rigid;
    public GameObject ThePlayer;
    public float TargetDistance;
    public float AllowedDistance = 1;
    public GameObject TheNPC;
    public float FollowSpeed;
    public RaycastHit Shot;
    private bool is_attacked=false;
    // public float reachDist = 20.0f;
    public int currentPoint = 0;

    /// <summary>
    /// This component provides the results of the AI system. These results are then applied to the attached <see
    /// cref="Animator"/>. Thus, the reference to an AIMContext component is mandatory. The controller is disabled
    /// if no Context instance can be found at OnEnable.
    /// </summary>
    [Tooltip("This component provides the results of the AI system. These results are then applied to the " +
        "attached Animator. Thus, the reference to an AIMContext component is mandatory.The controller is + " +
        "disabled if no Context instance can be found at OnEnable.")]
    public AIMContext Context;

    /// <summary>
    /// The maximum value of the parameter passed to the <see cref="AnimatorParameter"/> that is assumend to somehow
    /// control the movement animation. Thus, it could be seen as a limit for movement speed.
    /// </summary>
    public float MovementSpeed = 0.5f;

    /// <summary>
    /// Controls how fast the character can rotate to a direction given by the <see cref="Context"/>. In radians per
    /// second. For example, a value of 3.141 means that the character can turn around in one second.
    /// </summary>
    [Tooltip("Controls how fast the character can rotate to a direction given by the Context. In radians per " +
        "second. For example, a value of 3.141 means that the character can turn around in one second.")]
    public float RotationSpeed = 1.0f;

    /// <summary>
    /// If set equal to or greater than 0, the evaluated AI decision value is multiplied to the <see
    /// cref="MovementSpeed"/>.
    /// </summary>
    [Tooltip("If set equal to or greater than 0, the evaluated AI decision value is multiplied to the 'Speed'.")]
    [TargetObjective(true)]
    public int ObjectiveAsSpeed = -1;

    #endregion // Fields
        private void OnEnable()
        {
            if (Context == null /*|| Animator == null*/)
            {
                Debug.LogWarning('(' + typeof(RootMotionController).Name + ") " + name + ": deactivated because a " +
                    "reference to either an AIMContext or an Animator is missing.");
                enabled = false;
                return;
            }

           // Animator.applyRootMotion = true;
        }
        void OnTriggerEnter(Collider col)
            {
                triggering = true;
                triggerPlayer = col.gameObject;

            }
         void OnTriggerExit(Collider col)
            {
                if (col.tag == "Player")
                {
                    triggering = false;
                    triggerPlayer = null;
                }
            }

          private void Update()

        
            {

            Vector3 targetDirection = Context.DecidedDirection;
            float step = RotationSpeed * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, step, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
            //ThePlayer.GetComponent<Animator>().SetBool("Attack", true);

            float speedMultiplier = 1.0f;
            if (Vector3.Angle(targetDirection, transform.forward) > 50.0f)
                speedMultiplier = 0.0f;

            if (ObjectiveAsSpeed >= 0 && ObjectiveAsSpeed < Context.DecidedValues.Count)
            {

                float magnitude = Context.DecidedValues[ObjectiveAsSpeed] * MovementSpeed;
                magnitude = magnitude > MovementSpeed ? MovementSpeed : magnitude;
                transform.LookAt(ThePlayer.transform);
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out Shot))
                {
                        TargetDistance = Shot.distance;
                        if (TargetDistance >= AllowedDistance)
                        {
                            FollowSpeed = magnitude * speedMultiplier;
                            TheNPC.GetComponent<Animation>().Play("Walk");
                            transform.position = Vector3.MoveTowards(transform.position, ThePlayer.transform.position, FollowSpeed);
                            is_attacked = false;
                            ThePlayer.GetComponent<Animator>().SetBool("Attack", is_attacked);
                            }
                        else
                        {
                                is_attacked = true;
                                FollowSpeed = 0;
                               /* for (int n = 0; n <= 2; n++)
                                {
                                    TheNPC.GetComponent<Animation>().Play("Attack");
                                    if (n == 2)
                                         TheNPC.GetComponent<Animation>().Play("Idle");
                                }
                        //TheNPC.GetComponent<Animation>().Play("Attack");*/
                       /*  if (TheNPC.GetComponent<Animation>().Play("Attack"))
                         {
                            ThePlayer.GetComponent<Animator>().Play("DAMAGED00");
                             triggering = false;
                             Debug.Log("fffff");
                             if (Input.GetKeyDown(KeyCode.X))
                             {
                                // TheNPC.SetActive(true);
                                 TheNPC.GetComponent<Animation>().Stop("Attack");
                                 TheNPC.GetComponent<Animation>().Play("Death");
                                 triggering = false;
                                 Destroy(TheNPC.gameObject, 3f);
                                 Debug.Log("dddddxf");
                             }   

                 }
                 //ThePlayer.GetComponent<Animator>().SetBool("Attack", is_attacked);
                 /*  رجع تانى          ThePlayer.GetComponent<Animator>().Play("DAMAGED00");
                             if (Input.GetKeyDown(KeyCode.X))
                             { TheNPC.GetComponent<Animation>().Play("Death"); }لحد هنا*/
                        //if (Input.anyKeyDown)
                        //  { TheNPC.GetComponent<Animation>().Play("Idle"); }
                        // { ThePlayer.GetComponent<Animator>().SetBool("Attack", false); }
                    }
                        /*if (is_attacked == true)
                        {
                            ThePlayer.GetComponent<Animator>().Play("DAMAGED00", -1, 0f);
                        }*/
                       // else { }
                }
                /*   Animator.SetFloat("Speed", magnitude * speedMultiplier);
                   var look = GameObject.FindGameObjectWithTag("player").transform;
                   float dist = Vector3.Distance(/*player1[currentPoint]-----look.position, transform.position);
                   transform.position = Vector3.forward;
                   //transform.position = Vector3.MoveTowards(transform.position, player1[currentPoint].position, Time.deltaTime * MovementSpeed);
                   if (dist <= reachDist)
                   {
                       currentPoint++;
                   }
                   if (currentPoint >= player1.Length)
                   {
                       currentPoint = 0;
                   }*/
            }
            else
            {
                transform.LookAt(ThePlayer.transform);
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out Shot))
                {
                        TargetDistance = Shot.distance;
                        if (TargetDistance >= AllowedDistance)
                        {
                            FollowSpeed = 0.05f;
                            TheNPC.GetComponent<Animation>().Play("Walk");
                            transform.position = Vector3.MoveTowards(transform.position, ThePlayer.transform.position, FollowSpeed);
                            is_attacked = false;
                            ThePlayer.GetComponent<Animator>().SetBool("Attack", is_attacked);
                            }
                        else
                        {
                                is_attacked = true;
                                FollowSpeed = 0;
                        //فكره جديجده
                              /*  for (int n = 0; n <= 3; n++)
                                {
                                    TheNPC.GetComponent<Animation>().Play("Attack");
                                    if (n == 2)
                                        TheNPC.GetComponent<Animation>().Play("Idle");

                                }

                                //ThePlayer.GetComponent<Animator>().SetBool("Attack",is_attacked);
                                //ThePlayer.GetComponent<Animator>().Play("DAMAGED00");*/
                               /* if (TheNPC.GetComponent<Animation>().Play("Attack"))
                                {
                                    ThePlayer.GetComponent<Animator>().Play("DAMAGED00");
                                    triggering = false;
                                    Debug.Log("fffff");
                                    if (Input.GetKeyDown(KeyCode.X))
                                    {
                                        //TheNPC.SetActive(true);
                                        TheNPC.GetComponent<Animation>().Stop("Attack");
                                        TheNPC.GetComponent<Animation>().Play("Death");
                                        triggering = false;
                                        Destroy(TheNPC.gameObject, 3f);
                                        Debug.Log("dddddxf");
                                    }
                                }*/
                        //ThePlayer.GetComponent<Animator>().SetBool("Attack", false);
                        /******     if (Input.GetKeyDown(KeyCode.X))
                             { TheNPC.GetComponent<Animation>().Play("Death"); }*******/

                    }
                       /* if (is_attacked == true)
                        {
                            ThePlayer.GetComponent<Animator>().Play("DAMAGED00");
                        }
                        if (Input.GetKeyDown(KeyCode.UpArrow))
                        { ThePlayer.GetComponent<Animator>().SetBool("Attack", is_attacked); }*/
                    }
                    // Animator.SetFloat("Speed", MovementSpeed * speedMultiplier);
                

                    /* float dist = Vector3.Distance(player1[currentPoint].position, transform.position);
                     transform.position = Vector3.MoveTowards(transform.position, player1[currentPoint].position, Time.deltaTime * MovementSpeed);
                     if (dist <= reachDist)
                     {
                         currentPoint++;
                     }
                     if (currentPoint >= player1.Length)
                     {
                         currentPoint = 0;
                     }*/
                }

            #region Methods ================================================================================================

            if (TargetDistance <= AllowedDistance)
            {
                    TheNPC.GetComponent<Animation>().Play("Attack");
				    //ScoreScript.ScoreValue-=5;
                    //ThePlayer.GetComponent<Animator>().Play("DAMAGED00");
                    triggering = false;
                    Debug.Log("fffff");

            }
            
            #endregion // Methods
        }
        
    } // class RootMotionController
    } // namespace Polarith.AI.Package
